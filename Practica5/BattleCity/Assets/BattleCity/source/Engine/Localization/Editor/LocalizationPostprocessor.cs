using System.IO;
using UnityEditor;
using UnityEngine;

public class LocalizationPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        //Debug.Log("Fichero cargado " + LocalizationMgr.Instance.FiledLoaded);
        for (int i = 0; i < importedAssets.Length; ++i)
        {

            if (importedAssets[i].EndsWith("LocalizationConfig.asset"))
            {
                LocalizationMgr.Reload();
                Debug.Log("recargando por cambio del scriptableobject de la configuración");
            }
            if (LocalizationMgr.Instance.IsFileLoaded(Path.GetFileNameWithoutExtension(importedAssets[i])))
            {
                LocalizationMgr.Reload();
                Debug.Log("recargando por por cambio del fichero que tenemos cargado en memoria");
            }
        }
        /*if (assetImporter.importSettingsMissing)
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
            if (modelImporter != null)
            {
                if (!assetPath.Contains("@"))
                    modelImporter.importAnimation = false;
                modelImporter.importMaterials = false;
            }
        }*/
    }

}
