from MLP import MLP, target_gradient, costNN, MLP_backprop_predict
from utils import load_data, load_weights,one_hot_encoding, accuracy
from public_test import checkNNGradients,MLP_test_step
from sklearn.model_selection import train_test_split
from sklearn.neural_network import MLPClassifier
from sklearn.metrics import accuracy_score
from MLP_Complete import MLP_Complete




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


def SKLearn_test(x_train, y_train, x_test, y_test, n_hidden_layers, lambda_, alpha, iterations):
    mlp = MLPClassifier(hidden_layer_sizes=(n_hidden_layers),
                            activation='logistic',      # funcion de activacion logistica (sigmoidal)
                            solver='sgd',               # solver estocastico
                            alpha=lambda_,              # alpha
                            learning_rate_init=alpha,        # 
                            max_iter=iterations, 
                            shuffle=True,
                            random_state=42)
    mlp.fit(x_train, y_train)
    y_pred = mlp.predict(x_test)
    accuracy = accuracy_score(y_test, y_pred)
    print(f"SKLearn calculate accuracy for lambda = {(lambda_):1.5f} : {(accuracy):1.5f}")

def MLP_Complete_Test(x_train, y_train, x_test, y_test, n_hidden_layers, lambda_, alpha, iterations):
    mlp = MLP_Complete(x_train.shape[1],[25],y_train.shape[1])
    Jhistory = mlp.backpropagation(x_train,y_train,alpha,lambda_,iterations)
    a, z = mlp.feedforward(x_test)
    a3 = a[-1]             # coge la ultima
    y_pred=mlp.predict(a3)
    accuracy = accuracy_score(y_test, y_pred)
    print(f"MLP Complete calculate accuracy for lambda = {(lambda_):1.5f} : {(accuracy):1.5f}")

def main():
    print("Main program")

    # -----------------------------------------------------------------------
    # EJERCICIOS 1 y 2
    fluttershy = MLP(400, 25, 10)

    # TEST
    #gradientTest()

    # -----------------------------------------------------------------------
    # EJERCICIO 3
    # carga los datos
    X, Y = load_data('./data/ex3data1.mat')

    # coge una parte aleatoria de los datos ()
    # metodo de sklearn
    x_train, x_test, y_train, y_test = train_test_split(X, Y, test_size=0.33, shuffle=True, random_state=0)

    # aplicar el one hot encoding con los datos de entrenamiento (y_train) 
    yohe_train = one_hot_encoding(y_train)

    #TEST 2
    MLP_test(x_train, yohe_train, x_test, y_test)

    # -----------------------------------------------------------------------
    # EJERCICIO 4 (MLPClassifier SKLearn)

    lambda_ = 0         # 
    num_iters = 2000
    alpha = 0.01        # learning rate
    n_hidden_layers = 25
    
    # TEST 1
    #SKLearn_test(x_train, y_train, x_test, y_test, n_hidden_layers, lambda_, alpha, num_iters)

    # TEST 2
    lambda_ = 0.5
    #SKLearn_test(x_train, y_train, x_test, y_test, n_hidden_layers, lambda_, alpha, num_iters)

    # TEST 3
    lambda_ = 1    
    #SKLearn_test(x_train, y_train, x_test, y_test, n_hidden_layers, lambda_, alpha, num_iters)


    # -----------------------------------------------------------------------
    # EJERCICIO 5 (ya no opcional lmmaooo)
    justb = MLP_Complete(400, [100, 50, 25], 10)

    # TEST 1
    lambda_ = 0         # 
    num_iters = 2000
    alpha = 0.01        # learning rate
    n_hidden_layers = 25
    
    # TEST 1
    MLP_Complete_Test(x_train, yohe_train, x_test, y_test, n_hidden_layers, lambda_, alpha, num_iters)

    # TEST 2
    lambda_ = 0.5
    #MLP_Complete_Test(x_train, yohe_train, x_test, y_test, n_hidden_layers, lambda_, alpha, num_iters)

    # TEST 3
    lambda_ = 1    
    #MLP_Complete_Test(x_train, yohe_train, x_test, y_test, n_hidden_layers, lambda_, alpha, num_iters)

    #SKLearn_test(x_train, y_train, x_test, y_test, n_hidden_layers, lambda_, alpha, num_iters)

main()