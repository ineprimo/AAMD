import glob as glob
import pandas as pd


# LECTURA DE DATOS
#coge todos los csv de la carpeta que tenga 
pathnames_unsorted = glob.glob(".\\CSVs\\TankTraining*.csv")

# los ordena
pathnames = sorted(pathnames_unsorted)

lines = 0

print("files sorted")
# abre los archivos y guarda las lineas (no creo que sea necesario)
for f in pathnames:
    file = open(f, 'r', encoding="utf-8")
    n = file.readlines()
    lines += len(n)

# para que no cuente la de win
filas = lines - len(pathnames)
# falta otra?

# --------------------------------------------------
# JUNTANDO DATOS
data_result = []
print(pathnames)
for f in pathnames:
    aux = pd.read_csv(f)        # lee el csv
    aux2 = aux.iloc[:-1]        # quita el win/lose
    # ¿?
    data_result.append(aux2)


clean_data = pd.concat(data_result, ignore_index=True) # metiendo el dato limpio en la lista

# lista de columnas que queremos quitar
unnecessary_columns = [
    "COMMAND_CENTER_X", 
    "COMMAND_CENTER_Y", 
    "CAN_FIRE", 
    "LIFE_X", 
    "LIFE_Y", 
    "HEALTH"
    ]

final_data = clean_data.drop(columns=unnecessary_columns) 


final_data.to_csv("./datosLimpios.csv", index=False)



# ONE HOT ENCODIGN !!!

    