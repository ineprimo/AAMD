using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



/// <summary>
/// Clase utilizada para pintar en el editor el dropdown para elegir entre las diferentes opciones del localizador
/// dadas por el CSV con la información de las mismas.
/// 
/// El contenido mostrado está guardado en una variable estática en la clase LocalizeInspectorStr y es accesible
/// a través de la variable "GUIContenArray". Para guardar la opción seleccionada, usamos la clase SerializedProperty
/// y guardamos la selección con la palabra clave "key".
/// 
/// Lo primero que se hace es comprobar que el contenido a mostrar no es nulo. En caso de serlo, probablemente sea
/// por que es la primera vez que se llama a la función. En cualquier caso, se llama al método usado para actualizar
/// la lista de elementos de la GUI, los cuales se guardan en la variable ya mencionada "GUIContenArray", y se añaden
/// las funciones de actualizar la lista y la de destruirla a los eventos del LocalizationMgr para actualizarla o destruirla
/// cuando proceda.
/// 
/// Una vez con la lista actualizada, se obtiene el valor seleccionado previamente por el usuario y se lanza el dropdown
/// (mediante la función Popup del editor de unity) en la posición deseada, con la selección y con el contenido de la lista,
/// el cual es un array de GUIContent cada uno con su texto a mostrar y su tooltip para cuando el usuario pasa el ratón por
/// encima.
/// 
/// Tras ello, se serializa la nueva selección del usuario y se añade al editor una caja de ayuda donde se muestra al usuario
/// la tooltip asociada a la opción escogida, para que pueda verla en todo momento sin necesidad de pasar el ratón por encima
/// del dropdown.
/// </summary>
[CustomPropertyDrawer(typeof(LocalizeInspectorPopup))]
public class LocalizeInspectorPopupPropertyDrawer : PropertyDrawer
{
    private const float HEIGHT_SCALE = 1.5f;

    /// <summary>
    /// Función llamada para mostrar el dropdown con la lista de opciones del localizador en el editor.
    /// </summary>
    /// <param name="position">Posición en el editor de la GUI que se va a mostrar</param>
    /// <param name="property">Propiedad que estamos serializando/deserializando</param>
    /// <param name="label">Etiqueta de la gui donde va el tooltip, name, etc</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        if (LocalizeInspectorStr.GuiContentIsNull)
        {
            LocalizeInspectorStr.RefreshList();
            LocalizationMgr.onReloadList += LocalizeInspectorStr.RefreshList;
            LocalizationMgr.onDestroy += LocalizeInspectorStr.OnDestroyManager;
        }

        SerializedProperty k = property.FindPropertyRelative("key");
        int selected = FindSelected(k.stringValue);

        if(selected < 0)
        {
            Debug.LogError("Error estas buscando una key que no existe: "+ k.stringValue);
            selected = 0;
        }

        var textRect = new Rect(position.x, position.y, position.width, position.height);
        var helpRect = new Rect(position.x, position.y + 20f, position.width, position.height);

        selected = EditorGUI.Popup(textRect, selected, LocalizeInspectorStr.GUIContentArray);

        if (selected >= 0 && k != null && selected < LocalizeInspectorStr.GUIContentArray.Length)
            k.stringValue = LocalizeInspectorStr.GUIContentArray[selected].text;
        else
            k.stringValue = null;

        EditorGUI.EndProperty();
    }
    
    /// <summary>
    /// Función usada para localizar la posición en la lista de opciones del localizador dada una clave
    /// </summary>
    /// <param name="key">Clave a buscar en la lista de opciones</param>
    /// <returns>Devuelve la posición en la que se encuentra la clave. Si no la encuentra devuelve un -1</returns>
    public int FindSelected(string key)
    {
        //Esta es la primera vez que leemos la clave, no hemos asignado aún ningún valor, así que asignamos por defecto la 0
        if(key == null || key == "")
            return 0;
        
        for(int i =0; i < LocalizeInspectorStr.GUIContentArray.Length; ++i)
        {
            if (LocalizeInspectorStr.GUIContentArray[i].text == key)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Función interna del propertyDrawer que podemos sobrescribir para cambiar el tamaño que el propertyDrawer tendrá en el 
    /// En el inspector. En nuestro caso, como tenemos el helpbox necesitamos darle más tamaño en vertical
    /// </summary>
    /// <param name="property">Propiedad que estamos serializando/deserializando</param>
    /// <param name="label">Etiqueta de la gui donde va el tooltip, name, etc</param>
    /// <returns></returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totalHeight = EditorGUI.GetPropertyHeight(property, label, true)*HEIGHT_SCALE + EditorGUIUtility.standardVerticalSpacing;
        return totalHeight;
    }

    
}



[CustomPropertyDrawer(typeof(LocalizeInspectorStr))]
public class LocalizeInspectorStrPropertyDrawer : PropertyDrawer
{
    private const float HEIGHT_SCALE = 4f;
    /// <summary>
    /// Función llamada para mostrar el dropdown con la lista de opciones del localizador en el editor.
    /// </summary>
    /// <param name="position">Posición en el editor de la GUI que se va a mostrar</param>
    /// <param name="property">Propiedad que estamos serializando/deserializando</param>
    /// <param name="label">Etiqueta de la gui donde va el tooltip, name, etc</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        if (LocalizeInspectorStr.GuiContentIsNull)
        {
            LocalizeInspectorStr.RefreshList();
            LocalizationMgr.onReloadList += LocalizeInspectorStr.RefreshList;
            LocalizationMgr.onDestroy += LocalizeInspectorStr.OnDestroyManager;
        }

        SerializedProperty k = property.FindPropertyRelative("key");

        var textRect = new Rect(position.x, position.y, position.width, position.height * 0.33f);
        var helpRect = new Rect(position.x, position.y + 20f, position.width, position.height * 0.66f);

        //selected = EditorGUI.Popup(textRect, selected, LocalizeInspectorStr.GUIContentArray);
        EditorGUI.PropertyField(textRect,k);
        if (k == null || k.stringValue == null || !LocalizationMgr.Instance.KeyExist(k.stringValue))
            EditorGUI.HelpBox(helpRect, "Clave incorrecta", MessageType.Warning);
        else
            EditorGUI.HelpBox(helpRect, LocalizationMgr.Instance.Translate(k.stringValue), MessageType.Info);


        EditorGUI.EndProperty();
    }


    /// <summary>
    /// Función interna del propertyDrawer que podemos sobrescribir para cambiar el tamaño que el propertyDrawer tendrá en el 
    /// En el inspector. En nuestro caso, como tenemos el helpbox necesitamos darle más tamaño en vertical
    /// </summary>
    /// <param name="property">Propiedad que estamos serializando/deserializando</param>
    /// <param name="label">Etiqueta de la gui donde va el tooltip, name, etc</param>
    /// <returns></returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totalHeight = EditorGUI.GetPropertyHeight(property, label, true) * HEIGHT_SCALE + EditorGUIUtility.standardVerticalSpacing;
        return totalHeight;
    }


}
