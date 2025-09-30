import numpy as np
import pandas as pd


def cleanData(data):

    data["score"] = data["score"].apply(lambda x:  str(x).replace(",","."))
    data = data.drop(data[data["user score"] == "tbd"].index)
    data["user score"] = data["user score"].apply(lambda x:  str(x).replace(",","."))
    data["score"] = data["score"].astype(np.float64)
    data["user score"] = data["user score"].astype(np.float64)*10
    return data

def cleanDataMulti(data):
    data = cleanData(data)
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

    # normaliza el array
    X_norm = (X - X.min()) / (X.max() - X.min())

    # media de cada columna
    mu = np.ndarray(shape=(X))

    # desviacion de cada columna
    sigma = np.ndarray()

    # find the mean of each column/feature
    # mu will have shape (n,)
    # find the standard deviation of each column/feature
    # sigma will have shape (n,)
    # element-wise, subtract mu for that column from each example,
    # divide by std for that column
    return X_norm, mu, sigma

def load_data_csv_multi(path,x1_colum,x2_colum,x3_colum,y_colum):
    data = pd.read_csv(path)
    data = cleanDataMulti(data)
    x1 = data[x1_colum].to_numpy()
    x2 = data[x2_colum].to_numpy()
    x3 = data[x3_colum].to_numpy()
    X = np.array([x1, x2, x3])
    X = X.T
    y = data[y_colum].to_numpy()
    return X, y

def GetNumGradientsSuccess(w1,w1Sol,b1,b1Sol):
    iterator = 0
    for i in range(len(w1)): 
        if np.isclose(w1[i],w1Sol[i]):
                iterator += 1
    if np.isclose(b1,b1Sol):
        iterator += 1
    return iterator