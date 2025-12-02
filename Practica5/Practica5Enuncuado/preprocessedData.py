from sklearn.preprocessing import OneHotEncoder, StandardScaler 
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
    "NEIGHBORHOOD_LEFT",
    "action"                # diria que esto va aqui porque en el fiondo es un enum pero no se si tienen que ser coherentes
]

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
encoder_final = encoder.fit_transform(encoder_data)  

# SCALER
scaler = StandardScaler()
scaler_data = data[standardScaling_columns]
scaler_final = scaler.fit_transform(scaler_data)

# le mete a final_data los cambios hechos 
# primero hacemos una copia (porque soy un poco paranoica)
final_data = data


# ESTA PARTE ESTA MAL DE MOMENTO
# le mete el one hot
#final_data[oneHot_columns] = encoder_final     # esto no va :( con lo comfy que era
final_data.drop(columns=oneHot_columns)
df_encoder = pd.DataFrame(data=encoder_data,    # valores del encoder
                            index=data.index,    # coge los indices de los datos
                            columns=encoder.get_feature_names_out(oneHot_columns))  # las columnas del one hot
final_data = pd.concat([df_encoder], axis=1)

# le mete el scaler
final_data[standardScaling_columns] = scaler_final

# las guarda en un csv con lo
final_data.to_csv("preprocessedData.csv", index=False)


