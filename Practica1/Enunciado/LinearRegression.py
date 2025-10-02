import numpy as np
import copy
import math

class LinearReg:
    """
    Computes the cost function for linear regression.

    Args:
        x (ndarray): Shape (m,) Input to the model
        y (ndarray): Shape (m,) the real values of the prediction
        w, b (scalar): Parameters of the model
    """
    def __init__(self, x, y, w, b):
        #(scalar): Parameters of the model
        self.x = x  # ndarray   -> input of the model
        self.y = y  # ndarray   -> real values
        self.w = w  # escalar   -> parameter w
        self.b = b  # escalar   -> parameter b

    """
    Computes the linear regression function.

    Args:
        x (ndarray): Shape (m,) Input to the model
    
    Returns:
        the linear regression value
    """
    def f_w_b(self, x):
        mult = np.multiply(self.w, x)
        return mult + self.b


    """
    Computes the cost function for linear regression.

    Returns
        total_cost (float): The cost of using w,b as the parameters for linear regression
               to fit the data points in x and y
    """
    def compute_cost(self):
        
        y_prima = self.f_w_b(self.x)

        # DUDA!!!! la media de la formula es de los datos reales? yo supongo que si
        # pero no lo tengo muy claro
        aux = self.y - y_prima
        s = np.square(aux)
        sumatorio = np.sum(s)
        cost = sumatorio/(np.size(self.y)*2)

        return cost
    

    """
    Computes the gradient for linear regression 
    Args:

    Returns
      dj_dw (scalar): The gradient of the cost w.r.t. the parameters w
      dj_db (scalar): The gradient of the cost w.r.t. the parameter b     
     """
    def compute_gradient(self):

        # DUDA !!! qie coojones es el [x sub i] que hay en el enunciado??? porque
        # entiendo que [y sub i] es la prediccion e [y sub i prima] es el valor real,
        # que se traduce en x e y en esta clase, pero entonces que es [x sub i] ¿?

        y_prima = self.f_w_b(self.x)

        # FALTA EL *Xi
        gradientw = (np.sum((y_prima - self.y)*self.x))/np.size(self.y)
        gradientb = (np.sum(y_prima -self.y))/np.size(self.y)
        
        # hay un metodo en numpy lmao
        #gradientw = np.gradient(self.x, self.w)
        #gradientb = np.gradient(self.x, self.b)
        return gradientw, gradientb
    
    
    """
    Performs batch gradient descent to learn theta. Updates theta by taking 
    num_iters gradient steps with learning rate alpha

    Args:
      alpha : (float) Learning rate
      num_iters : (int) number of iterations to run gradient descent
    Returns
      w : (ndarray): Shape (1,) Updated values of parameters of the model after
          running gradient descent
      b : (scalar) Updated value of parameter of the model after
          running gradient descent
      J_history : (ndarray): Shape (num_iters,) J at each iteration,
          primarily for graphing later
      w_initial : (ndarray): Shape (1,) initial w value before running gradient descent
      b_initial : (scalar) initial b value before running gradient descent
    """
    def gradient_descent(self, alpha, num_iters):
        # An array to store cost J and w's at each iteration — primarily for graphing later
        J_history = []
        w_history = []
        w_initial = copy.deepcopy(self.w)  # avoid modifying global w within function
        b_initial = copy.deepcopy(self.b)  # avoid modifying global w within function
        #TODO: gradient descent iteration by m examples.
        
        for i in num_iters:
            # el gradient descent se calcula con el gradiente del metodo de arriba
            self.w = w_initial - (alpha*self.compute_gradient())
            self.b = b_initial - (alpha*self.compute_gradient())

            # j_history guarda los costes por gradiente en cada iteracion
            J_history[i] = self.compute_cost()

        return self.w, self.b, J_history, w_initial, b_initial


def cost_test_obj(x,y,w_init,b_init):
    lr = LinearReg(x,y,w_init,b_init)
    cost = lr.compute_cost()
    return cost

def compute_gradient_obj(x,y,w_init,b_init):
    lr = LinearReg(x,y,w_init,b_init)
    dw,db = lr.compute_gradient()
    return dw,db
