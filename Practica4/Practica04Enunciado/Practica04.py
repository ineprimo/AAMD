from MLP import MLP, target_gradient, costNN, MLP_backprop_predict
from utils import load_data, load_weights,one_hot_encoding, accuracy
from public_test import checkNNGradients,MLP_test_step
from sklearn.model_selection import train_test_split



"""
Test 1 to be executed in Main
"""
def gradientTest():
    checkNNGradients(costNN,target_gradient,0)
    checkNNGradients(costNN,target_gradient,1)


"""
Test 2 to be executed in Main
"""
def MLP_test(X_train,y_train, X_test, y_test):
    print("We assume that: random_state of train_test_split  = 0 alpha=1, num_iterations = 2000, test_size=0.33, seed=0 and epislom = 0.12 ")
    print("Test 1 Calculando para lambda = 0")
    MLP_test_step(MLP_backprop_predict,1,X_train,y_train,X_test,y_test,0,2000,0.92606,2000/10)
    print("Test 2 Calculando para lambda = 0.5")
    MLP_test_step(MLP_backprop_predict,1,X_train,y_train,X_test,y_test,0.5,2000,0.92545,2000/10)
    print("Test 3 Calculando para lambda = 1")
    MLP_test_step(MLP_backprop_predict,1,X_train,y_train,X_test,y_test,1,2000,0.92667,2000/10)



def main():
    print("Main program")
    #Test 1

    # ejercios 1 y 2
    fluttershy = MLP(400, 25, 10)
    #gradientTest()

    # ejercicio 3
    # carga los datos
    X, Y = load_data('./data/ex3data1.mat')

    # coge una parte aleatoria de los datos ()
    # metodo de sklearn
    x_train, x_test, y_train, y_test = train_test_split(X, Y, test_size=0.33, shuffle=True, random_state=0)

    # aplicar el one hot encoding con los datos de entrenamiento (y_train) 
    yohe_train = one_hot_encoding(y_train)

    #Test 2
    MLP_test(x_train, yohe_train, x_test, y_test)

    

main()