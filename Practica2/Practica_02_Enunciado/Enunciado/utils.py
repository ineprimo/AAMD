import numpy as np
import pandas as pd


def cleanData(data):
    data["score"] = data["score"].apply(lambda x:  str(x).replace(",","."))
    data = data.drop(data[data["user score"] == "tbd"].index)
    data["user score"] = data["user score"].apply(lambda x:  str(x).replace(",","."))
    data["score"] = data["score"].astype(np.float64)
    data["user score"] = data["user score"].astype(np.float64)
    data["critics"] = data["critics"].astype(np.float64)
    data["users"] = data["users"].astype(np.float64)
    return data

def load_data_csv(path,x_colum,y_colum):
    data = pd.read_csv(path)
    data = cleanData(data)
    X = data[x_colum].to_numpy()
    y = data[y_colum].to_numpy()
    return X, y

def zscore_normalize_features(X):
    """
    computes  X, zcore normalized by column

    Args:
      X (ndarray (m,n))     : input data, m examples, n features

    Returns:
      X_norm (ndarray (m,n)): input normalized by column
      mu (ndarray (n,))     : mean of each feature
      sigma (ndarray (n,))  : standard deviation of each feature
    """
    #X_norm, mu, sigma = 0
      # media de cada columna
    # mu = np.ndarray(shape=(X))
    mu = np.mean(X, axis=0)

    # desviacion de cada columna
    sigma = np.std(X, axis=0, ddof=0)
    #sigma = np.ndarray()

    X_norm = (X - mu)/sigma


    return X_norm, mu, sigma

def load_data_csv_multi(path,x1_colum,x2_colum,x3_colum,y_colum):
    data = pd.read_csv(path)
    data = cleanData(data)
    x1 = data[x1_colum].to_numpy()
    x2 = data[x2_colum].to_numpy()
    x3 = data[x3_colum].to_numpy()
    X = np.array([x1, x2, x3])
    X = X.T
    y = data[y_colum].to_numpy()
    return X, y

    
## 0 Malo, 1 Bueno
def load_data_csv_multi_logistic(path,x1_colum,x2_colum,x3_colum,y_colum):
    X,y = load_data_csv_multi(path,x1_colum,x2_colum,x3_colum,y_colum)
    #TODO convertir la a clases 0,1.
    for i in range(y.shape[0]):
        if(i < 7):
            i = 0
        else:
            i = 1
    #y = np.where(y < 7, 0, 1)
    return X,y

## 0 Malo, 1 Regular, 2 Notable, 3 Sobresaliente, 4 Must Play.
def load_data_csv_multi_logistic2(path,x1_colum,x2_colum,x3_colum,y_colum):
    X,y = load_data_csv_multi(path,x1_colum,x2_colum,x3_colum,y_colum)
    #TODO convertir la a clases 0,1.
    for i in range(y.shape[0]):
        if(i < 5):
            i = 0
        elif (i > 5 and i < 7):
            i = 1
        elif (i > 7 and i < 9):
            i = 2
        elif (i > 9 and i < 9.5):
            i = 3
        else:
            i = 4
    #y = np.where(y < 7, 0, 1)
    return X,y

    
        