from sklearn.preprocessing import OneHotEncoder, StandardScaler, LabelEncoder
import pandas as pd

# Normalizacion de los datos:
# para datos enumerados -> OneHotEncoding, que los pasa a datos numericos
# para datos continuos -> StandardScaling, normaliza los datos 


# he hecho copy paste del csv lmao
# las columnas que vayan por enumerado 
oneHot_columns = [
    "NEIGHBORHOOD_UP",
    "NEIGHBORHOOD_DOWN",
    "NEIGHBORHOOD_RIGHT",
    "NEIGHBORHOOD_LEFT"
]

labeled_columns =  "action"        # antes lo tenia en el oneHot pero no tiene sentido porque esta es la solucion que estamos buscado

# las columnas que vayan con datos continuos
standardScaling_columns = [
    "NEIGHBORHOOD_DIST_UP",
    "NEIGHBORHOOD_DIST_DOWN",
    "NEIGHBORHOOD_DIST_RIGHT",
    "NEIGHBORHOOD_DIST_LEFT",
    "AGENT_1_X",
    "AGENT_1_Y",
    "AGENT_2_X",
    "AGENT_2_Y",
    "EXIT_X",
    "EXIT_Y",
    "time"
]

# lee los datos
data = pd.read_csv("cleanData.csv")


# ONE HOT ENCONDER 
encoder = OneHotEncoder(sparse_output=False)   # queremos que sea sparse?¿
encoder_data = data[oneHot_columns] # cogemos solo las columnas que queremos
encoder_final = encoder.fit_transform(encoder_data)  #AQUI ESTA EL PROBLEMA
print(encoder_final.shape)

# SCALER
scaler = StandardScaler()
scaler_data = data[standardScaling_columns]
scaler_final = scaler.fit_transform(scaler_data)

# LABELES (solucion)
labler = LabelEncoder()
labeled_final = labler.fit_transform(data[labeled_columns])

# le mete a final_data los cambios hechos 
# primero hacemos una copia (porque soy un poco paranoica)
temp_data = data


# le mete el one hot
final_data = temp_data.drop(columns=oneHot_columns)
oneHot_df = pd.DataFrame(encoder_final, columns=encoder.get_feature_names_out(oneHot_columns))  # las columnas del one hot
final_data = pd.concat([final_data, oneHot_df], axis=1)

final_data[standardScaling_columns] = scaler_final
#final_data[labeled_columns] = labeled_final

final_data[labeled_columns] = labeled_final

# las guarda en un csv con lo
final_data.to_csv("preprocessedData.csv", index=False)


