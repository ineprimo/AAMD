import numpy as np
from matplotlib import pyplot
from scipy.io import loadmat
from sklearn.preprocessing import OneHotEncoder
from sklearn.metrics import confusion_matrix

"""
Displays 2D data stored in X in a nice grid.
"""
def displayData(X, example_width=None, figsize=(10, 10)):

    # Compute rows, cols
    if X.ndim == 2:
        m, n = X.shape
    elif X.ndim == 1:
        n = X.size
        m = 1
        X = X[None]  # Promote to a 2 dimensional array
    else:
        raise IndexError('Input X should be 1 or 2 dimensional.')

    example_width = example_width or int(np.round(np.sqrt(n)))
    example_height = n / example_width

    # Compute number of items to display
    display_rows = int(np.floor(np.sqrt(m)))
    display_cols = int(np.ceil(m / display_rows))

    fig, ax_array = pyplot.subplots(
        display_rows, display_cols, figsize=figsize)
    fig.subplots_adjust(wspace=0.025, hspace=0.025)

    ax_array = [ax_array] if m == 1 else ax_array.ravel()

    for i, ax in enumerate(ax_array):
        ax.imshow(X[i].reshape(example_width, example_width, order='F'),
                  cmap='Greys', extent=[0, 1, 0, 1])
        ax.axis('off')

"""
Load data from the dataset.
"""
def load_data(file):
    data = loadmat(file, squeeze_me=True)
    x = data['X']
    y = data['y']
    return x,y

"""
Load weights from the weights file 
"""
def load_weights(file):
    weights = loadmat(file)
    theta1, theta2 = weights['Theta1'], weights['Theta2']
    return theta1, theta2


"""
Implementation of the one hot encoding... You must use OneHotEncoder function of the sklern library. 
Probably need to use reshape(-1, 1) to change size of the data
"""
def one_hot_encoding(Y):
    YEnc = 0
    #TO-DO: implements
    return YEnc

"""
Implementation of the accuracy metrics function
"""
def accuracy(P,Y):
	#TO-DO: implements
    m = len(P)
    cont = 0
    for i in range(m):
        if(P[i] == Y[i]):
            cont += 1

    return cont/m