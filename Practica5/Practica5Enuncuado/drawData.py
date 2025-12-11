import pandas as pd
import matplotlib.pyplot as plt

# ABRIR CON ANACONDAAAAA
from sklearn.decomposition import PCA
from sklearn.preprocessing import StandardScaler

import matplotlib.pyplot as plt
import numpy as np


data = pd.read_csv("preprocessedData.csv")  # abre el archivo de los datos limpios

action_cols = [col for col in data.columns if col.startswith('action')]

atributes = data.drop(columns=action_cols)

color = np.argmax(data[action_cols].values, axis=1)

# MOMENTO PCA: 
# [explicacion guapa]
scaling = StandardScaler()
scaling.fit(atributes)
scaled_atributes = scaling.transform(atributes)



# PCA
pca_comp = PCA(n_components=2)
hola = pca_comp.fit_transform(scaled_atributes)

plt.figure(figsize=(8,6))

scatter = plt.scatter(     
    hola[:, 0],
    hola[:, 1],
    c = color.astype(int),                  
    cmap = "twilight_r",         
    alpha = 0.7             
)
plt.colorbar(scatter, label="Hola Isma")

plt.show()