import matplotlib.pyplot as plt
import numpy as np
from matplotlib import pyplot
from scipy.io import loadmat
from sklearn.preprocessing import OneHotEncoder


###########################################################################
# data display
#
def displayData(X):
    num_plots = int(np.size(X, 0)**.5)
    fig, ax = plt.subplots(num_plots, num_plots, sharex=True, sharey=True)
    plt.subplots_adjust(left=0, wspace=0, hspace=0)
    img_num = 0
    for i in range(num_plots):
        for j in range(num_plots):
            # Convert column vector into 20x20 pixel matrix
            # transpose
            img = X[img_num, :].reshape(20, 20).T
            ax[i][j].imshow(img, cmap='Greys')
            ax[i][j].set_axis_off()
            img_num += 1

    return (fig, ax)


def displayImage(im):
    fig2, ax2 = plt.subplots()
    image = im.reshape(20, 20).T
    ax2.imshow(image, cmap='gray')
    return (fig2, ax2)



def load_data(file):
    data = loadmat(file, squeeze_me=True)
    x = data['X']
    y = data['y']
    return x,y

def load_weights(file):
    weights = loadmat(file)
    theta1, theta2 = weights['Theta1'], weights['Theta2']
    return theta1, theta2

def one_hot_encoding(Y):
    YEnc = 0
    #TO-DO: implements
    return YEnc

def accuracy(P,Y):
	#TO-DO: implements
	return -1


###########################################################################
# gradient checking
#
def debugInitializeWeights(fan_in, fan_out):
    """
    Initializes the weights of a layer with fan_in incoming connections and
    fan_out outgoing connections using a fixed set of values.
    """

    W = np.sin(np.arange(1, 1 + (1+fan_in)*fan_out))/10.0
    W = W.reshape(fan_out, 1+fan_in, order='F')
    return W


def computeNumericalGradient(J, Theta1, Theta2):
    """
    Computes the gradient of J around theta using finite differences and
    yields a numerical estimate of the gradient.
    """

    theta = np.append(Theta1, Theta2).reshape(-1)

    numgrad = np.zeros_like(theta)
    perturb = np.zeros_like(theta)
    tol = 1e-4

    for p in range(len(theta)):
        # Set perturbation vector
        perturb[p] = tol
        loss1 = J(theta - perturb)
        loss2 = J(theta + perturb)

        # Compute numerical gradient
        numgrad[p] = (loss2 - loss1) / (2 * tol)
        perturb[p] = 0

    return numgrad

