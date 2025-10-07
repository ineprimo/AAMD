import numpy as np
import copy
import math

from LinearRegression import LinearReg

class LinearRegMulti(LinearReg):

    """
    Computes the cost function for linear regression.

    Args:
        x (ndarray): Shape (m,) Input to the model
        y (ndarray): Shape (m,) the real values of the prediction
        w, b (scalar): Parameters of the model
        lambda: Regularization parameter. Most be between 0..1. 
        Determinate the weight of the regularization.
    """
    def __init__(self, x, y,w,b, lambda_):
        super().__init__(x, y, w, b)
        self.lamb = lambda_
        return

    def f_w_b(self, x):
        ret = x @ self.w + self.b
        return ret


    def compute_cost(self):
        
        y_prima = self.f_w_b(self.x)

        # para optimizar
        cost = super().compute_cost()

        


        return cost
    

    
    """
    Compute the regularization cost (is private method: start with _ )
    This method will be reuse in the future.

    Returns
        _regularizationL2Cost (float): the regularization value of the current model
    """
    
    def _regularizationL2Cost(self):
        return 0
    
    """
    Compute the regularization gradient (is private method: start with _ )
    This method will be reuse in the future.

    Returns
        _regularizationL2Gradient (vector size n): the regularization gradient of the current model
    """ 
    
    def _regularizationL2Gradient(self):
        return 0

    
def cost_test_multi_obj(x,y,w_init,b_init):
    lr = LinearRegMulti(x,y,w_init,b_init,0)
    cost = lr.compute_cost()
    return cost

def compute_gradient_multi_obj(x,y,w_init,b_init):
    lr = LinearRegMulti(x,y,w_init,b_init,0)
    dw,db = lr.compute_gradient()
    return dw,db
