using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilistRandomSelection 
{
    private float[] _acumulatedProbabilities;
    private int _memory;
    private int _lastOption;
    private int _amountLastOptions;
    private int _minSelectedInStreak;
    private Dictionary<int, int> _optionsSelected;
    private int _maxSelectionGuaranteeNew;
    private int _streak;
    private bool _resetStreak;

    /// <summary>
    /// Constructor que permite crear secuencias aparentemente aleatorias pero con ciertas limitaciones interesantes para el diseño de un juego y un comportamiento aleatorio de un enemigo o boss
    /// </summary>
    /// <param name="probalities"></param> lista de las probabilidades de las opciones disponibles, debe haber tantas como opciones. No tiene porque sumar 1 ya que lo normaliza internamente
    /// <param name="memory"></param> numero de veces que consentimos que una decisión se tome consecutivamente. Sea la que sea (no hilamos tan fino)
    /// <param name="maxSelectionGuaranteeNew"></param> garantizamos que en uan racha que indica en este paámetro, generaremos al menos un elemento de la opción minoritaria
    /// El numero que consideramos que minimo debe tener la opción minoritaria en la serie especificada en este atributo, se indica en el siguiente.
    /// <param name="minSelectedInStreak"></param>
    /// <param name="resetStreak"></param> Si es true cuando acaba la serie indicada en maxSelectionGuaranteeNew se resetea la cuenta, lo que nos permite que la menos aparezca 1
    /// una selección de todas las decisiones en el rango establecido
    public ProbabilistRandomSelection(float[] probalities, int memory, int maxSelectionGuaranteeNew, int minSelectedInStreak = 1, bool resetStreak = false)
    {
        _acumulatedProbabilities = _.AcumulateProb(probalities);
        _memory = memory;
        _maxSelectionGuaranteeNew = maxSelectionGuaranteeNew;
        _optionsSelected = new Dictionary<int, int>();
        _minSelectedInStreak = minSelectedInStreak;
        _resetStreak = resetStreak;
        Reset();
    }

    /// <summary>
    /// Selección aleatoria basada en probabilidad y no condicionada por la depuración.
    /// </summary>
    /// <returns></returns>
    protected int RandomSelect()
    {
        float r = Random.Range(0f, 1f);
        for (int i = 0; i < _acumulatedProbabilities.Length; i++)
        {
            if (r <= _acumulatedProbabilities[i])
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Select interno recursivo.
    /// </summary>
    /// <param name="debug"></param> para hacer pruebas
    /// <param name="recursive"></param> contador de veces que se intenta sacar otor número
    /// <param name="max"></param> máximo numero de intentos de sacar otro numero, por seguridad por si diese la casualidad que tarda muhcisimo en sacar otro numero
    /// <returns></returns>
    protected int _Select(int debug, int recursive, int max = 10)
    {
        int selected = debug;
        if (selected < 0)
        {
            selected = RandomSelect();
        }
        if (recursive > max)
            return selected;

        if (_maxSelectionGuaranteeNew > 0)
        {
            _streak++;
            if (_streak >= _maxSelectionGuaranteeNew)
            {
                int min;
                selected = GetLowerAppearance(selected, out min);
                if (min >= _minSelectedInStreak)
                {
                    _streak = 1;
                    if(_resetStreak)
                        ResetAppearance();
                }
            }
        }

        if (_memory > 0)
        {
            if (selected == _lastOption)
            {
                _amountLastOptions++;
                if (_amountLastOptions > _memory)
                {
                    //no vale, hemos repetido demasiadas veces una acción, elegimos otra.
                    selected = _Select(-1, recursive+1, max);
                }
            }
            else
            {
                _amountLastOptions = 1;
                _lastOption = selected;
            }
        }

        _optionsSelected[selected] += 1;
        return selected;
    }

    /// <summary>
    /// Select publico con opción para pruebas
    /// </summary>
    /// <param name="debug"></param>
    /// <returns></returns>
    public int Select(int debug = -1)
    {
        return _Select(debug, 0);
    }

    /// <summary>
    /// Resetea la serie aleatoria
    /// </summary>
    public void Reset()
    {
        _lastOption = -1;
        _amountLastOptions = 0;
        _streak = 0;

        ResetAppearance();
    }

    public int GetLowerAppearance(int selected, out int min)
    {
        min = int.MaxValue;
        int index = -1;

        foreach (var pair in _optionsSelected)
        {
            if(pair.Value < min)
            {
                min = pair.Value;
                index = pair.Key;
            }
        }
        if(min >= _minSelectedInStreak) // si el minimo es mayor que el minimo establecido, es que la serie se ha completado, podemos resetearla
        {
            return selected;
        }
        else
            return index;
    }

    public void ResetAppearance()
    {
        _optionsSelected.Clear();
        for (int i = 0; i < _acumulatedProbabilities.Length; i++)
        {
            _optionsSelected.Add(i, 0);
        }
    }
}
