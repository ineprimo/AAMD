import numpy as np
import math

class MLP_Complete:

    """
    Constructor: Computes MLP_Complete.
    """
    def __init__(self,inputLayer, hiddenLayers, outputLayer, seed=0, epislom = 0.12):
        self.inputLayer = inputLayer
        self.hiddenLayer = hiddenLayers
        self.outputLayer = outputLayer
        self.epsilom = epislom
        self.seed = seed
        
        if seed != 0:
            np.random.seed(seed)

        # sizes de las capas en una lista
        self.layer_size = [inputLayer]
        for i in hiddenLayers:             
            self.layer_size += [i]
        self.layer_size += [outputLayer]

        # prepara las thetas
        self.thetas = []
        self.new_trained(self.thetas, self.epsilom)

    """
    Reset the theta matrix created in the constructor
    """
    def new_trained(self,thetas,epsilom):
        thetas.clear() # Limpiamos por si acaso
        for i in range(len(self.layer_size) - 1):
            in_layer = self.layer_size[i]
            out_layer = self.layer_size[i + 1]
            # (Salida, Entrada + 1 bias)
            theta = np.random.uniform(-epsilom, epsilom, (out_layer, in_layer + 1))
            thetas.append(theta)
        
    def _size(self,x):
        return x.shape[0]
    
    def _sigmoid(self,z):
        return 1 / (1 + np.exp(-z))

    def _sigmoidPrime(self,a):
        return a * (1 - a)

    """
    Run the feedforward neural network step
    """
    def feedforward(self,x):
        a = []
        z = []

        # Capa de entrada
        s = self._size(x)
        a1 = np.hstack([np.ones((s, 1)), x])
        a.append(a1) # a[0]

        # Capas ocultas y salida
        for i, theta in enumerate(self.thetas):
            z_aux = a[-1] @ theta.T
            z.append(z_aux) # z[0] corresponde a la entrada de la capa 1 (oculta 1)
            
            a_aux = self._sigmoid(z_aux)
            
            # Si no es la última capa (capa de salida), añadimos bias
            if i < len(self.thetas) - 1:
                s = self._size(a_aux)
                a_aux = np.hstack([np.ones((s, 1)), a_aux])
            
            a.append(a_aux)
            
        return a, z

    def compute_cost(self, yPrime, y, lambda_):
        m = y.shape[0]
        # Evitar log(0)
        epsilon = 1e-15
        yPrime = np.clip(yPrime, epsilon, 1 - epsilon)
        
        J = (-1/m) * np.sum(y * np.log(yPrime) + (1 - y) * np.log(1 - yPrime))
        J += self._regularizationL2Cost(m, lambda_)
        return J
    
    def predict(self,a3):
        p = np.argmax(a3, axis=1)
        return p
    

    """
    Compute the gradients of both theta matrix parameters and cost J
    ** CORREGIDO **
    """
    def compute_gradients(self, x, y, lambda_):
        m = self._size(x)    
        a, z = self.feedforward(x)
        
        J = self.compute_cost(a[-1], y, lambda_)

        # Inicializamos lista de gradientes del mismo tamaño que thetas
        gradientes = [np.zeros_like(theta) for theta in self.thetas]
        
        # Delta de la capa de salida
        # a[-1] es la salida final
        delta = a[-1] - y 
        
        # Recorremos las capas hacia atrás
        num_layers = len(self.thetas)
        
        for i in range(num_layers - 1, -1, -1):
            # i es el índice de la matriz theta que estamos ajustando
            
            # Calcular Gradiente para Theta[i]
            # a[i] es la activación de la capa anterior
            grad = (delta.T @ a[i]) / m
            reg = self._regularizationL2Gradient(self.thetas[i], lambda_, m)
            gradientes[i] = grad + reg
            
            # Calcular Delta para la siguiente iteración (capa anterior)
            # Solo si no estamos en la primera capa (input)
            if i > 0:
                # Quitamos la columna de bias de Theta para propagar el error hacia atrás
                theta_no_bias = self.thetas[i][:, 1:] 
                
                # z[i-1] es el z de la capa anterior
                sig_prime = self._sigmoidPrime(self._sigmoid(z[i-1]))
                
                delta = (delta @ theta_no_bias) * sig_prime

        return J, gradientes
    
    def _regularizationL2Gradient(self, theta, lambda_, m):
        reg_term = (lambda_ / m) * theta
        reg_term[:, 0] = 0  # No regularizar el bias (primera columna)
        return reg_term
    
    def _regularizationL2Cost(self, m, lambda_):
        cost = 0
        for theta in self.thetas:
            # Suma de cuadrados excluyendo la primera columna (bias)
            cost += np.sum(theta[:, 1:]**2)
        return (lambda_/(2*m)) * cost
    
    def backpropagation(self, x, y, alpha, lambda_, numIte, verbose=0):
        Jhistory = []
        for i in range(numIte):
            J, gradientes = self.compute_gradients(x, y, lambda_)
            
            # Actualizar pesos
            for k in range(len(self.thetas)):
                self.thetas[k] -= alpha * gradientes[k]
            
            Jhistory.append(J)
            
            if verbose > 0 :
                if i % verbose == 0 or i == (numIte-1):
                    print(f"Iteration {(i+1):6}: Cost {float(J):8.4f}   ")
        
        return Jhistory


def MLP_backprop_predict_complete(X_train, y_train, X_test, alpha, lambda_, num_ite, verbose):
    # Nota: hiddenLayers debe ser una lista, ej: [25]
    input_size = X_train.shape[1]
    output_size = y_train.shape[1]
    
    mlp = MLP_Complete(input_size, [25], output_size)
    
    Jhistory = mlp.backpropagation(X_train, y_train, alpha, lambda_, num_ite, verbose)
    
    a, z = mlp.feedforward(X_test)
    a3 = a[-1] # Última capa
    y_pred = mlp.predict(a3)
    
    return y_pred