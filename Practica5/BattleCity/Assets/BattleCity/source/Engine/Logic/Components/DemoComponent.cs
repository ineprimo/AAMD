using UnityEngine;

/// <summary>
/// Nos sirve principalmente para buscar los componentes que hay que modificar en las escenas para las demos
/// </summary>

//https://stackoverflow.com/questions/49127051/wheres-location-of-the-unitys-default-icon-for-scripts
[Icon(path: "MonoLogo")]
public class DemoComponent : MonoBehaviour
{
    [ReadOnlyInspector]
    [Tooltip("Este componente sirve para buscar los componentes que hay que modificar para hacer una demo el atributo extends simplemente indica que este componente here")]
    public string InheritedFrom = "DemoComponent";

}
