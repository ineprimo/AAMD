from utils import debugInitializeWeights, computeNumericalGradient
from sklearn.metrics import accuracy_score
import numpy as np

"""
Check that the gradients are correctly calculated.
"""
def checkNNGradients(costNN, target_gradient, reg_param=0):
    # Set up small NN
    input_layer_size = 3
    hidden_layer_size = 5
    num_labels = 3
    m = 5

    X = debugInitializeWeights(input_layer_size - 1, m)
    # Set each element of y to be in [0,num_labels]
    y = [(i % num_labels) for i in range(m)]

    ys = np.zeros((m, num_labels))
    for i in range(m):
        ys[i, y[i]] = 1
    
    cost, grad1, grad2, Theta1, Theta2  = target_gradient(input_layer_size,hidden_layer_size,num_labels,X,ys,reg_param)
    grad = np.concatenate((np.ravel(grad1), np.ravel(grad2)))

    def reduced_cost_func(p):
        """ Cheaply decorated nnCostFunction """
        Theta1 = np.reshape(
            p[:hidden_layer_size * (input_layer_size + 1)],
            (hidden_layer_size, (input_layer_size + 1)))
        Theta2 = np.reshape(
            p[hidden_layer_size * (input_layer_size + 1):],
            (num_labels, (hidden_layer_size + 1)))
        return costNN(Theta1, Theta2,
                      X, ys, reg_param)[0]

    numgrad = computeNumericalGradient(reduced_cost_func, Theta1, Theta2)

    # Check two gradients
    # np.testing.assert_almost_equal(grad, numgrad)

    # Evaluate the norm of the difference between two the solutions. If you have a correct
    # implementation, and assuming you used e = 0.0001 in computeNumericalGradient, then diff
    # should be less than 1e-9.
    diff = np.linalg.norm(numgrad - grad)/np.linalg.norm(numgrad + grad)

    print('If your compute_gradient implementation is correct, then \n'
          'the relative difference will be small (less than 1e-9). \n'
          'Relative Difference: %g' % diff)
    if diff < 1e-9:
        print("Test passed!")
    else:
        print("Error in your compute gradient implementation ")

"""
Check that MLP are correctly implemented.
"""
def MLP_test_step(mlp_backprop_predict,alpha,X_train,y_train, X_test, y_test, lambda_, num_ite, baseLineAccuracy, verbose=0):
    y_pred=mlp_backprop_predict(X_train,y_train,X_test,alpha,lambda_,num_ite,verbose)
    accu = accuracy_score(y_test, y_pred)
    print(f"Calculate accuracy for lambda = {(lambda_):1.5f} : {(accu):1.5f} expected accuracy is aprox: {(baseLineAccuracy):1.5f}")