from public_tests import *
from utils import *
from LinearRegression import LinearReg
from LinearRegression import cost_test_obj
from LinearRegression import compute_gradient_obj

from LinearRegressionMulti import LinearRegMulti
from LinearRegressionMulti import cost_test_multi_obj
from LinearRegressionMulti import compute_gradient_multi_obj
import pandas as pd
import numpy as np


# Functions to testing linear regression

def test_gradient_one(x_train, y_train):
      initial_w = 0
      initial_b = 0

      lr = LinearReg(x_train,y_train,initial_w,initial_b)
      lr_tmp_dj_dw, lr_tmp_dj_db = lr.compute_gradient()
      print('Gradient at initial w, b (similar to)(-50.49855985112268 -6.988606075159124):',
            lr_tmp_dj_dw, lr_tmp_dj_db)

      test_w = 0.2
      test_b = 0.2

      lr = LinearReg(x_train,y_train,test_w,test_b)
      lr_tmp_dj_dw, lr_tmp_dj_db = lr.compute_gradient()
      print('Gradient at initial w, b (similar to)(-38.700526233641476 -5.369312042261991):',
            lr_tmp_dj_dw, lr_tmp_dj_db)

      print("----Compute gradient-------")
      compute_gradient_test_one(compute_gradient_obj)

def test_cost_one(x_train, y_train):

      initial_w = 2
      initial_b = 1


      lr = LinearReg(x_train,y_train,initial_w,initial_b)
      lrcost = lr.compute_cost()
      print(type(lrcost))
      print(f'Cost at initial w (35.841): {lrcost:.3f}')
      print("----Compute cost-------")
      compute_cost_test_one(cost_test_obj)

def run_gradient_descent_one(x_train, y_train,alpha = 0.01,iterations=1500):
      # initialize fitting parameters. Recall that the shape of w is (n,)
      initial_w = 0.
      initial_b = 0.

      print("---- Gradient descent--")
      lr = LinearReg(x_train,y_train,initial_w,initial_b)
      w,b,h,w_init,b_init = lr.gradient_descent(alpha,iterations)
      print("w,b found by gradient descent (0.8262848855238131 1.0746825556592443):", w, b)

      return w, b


def test_gradient_descent_one(x_train, y_train, w, b):
      predict1 = 3.5 * w + b
      print('for score 3.5, we predict user score of (3.97) %.2f' %
            predict1)
      print(predict1)
      print("Case 1")
      assert np.allclose(predict1, 3.96667965 )
      print("Case 1 passed!")

      predict2 = 7.0 * w + b
      print('for score 7.9, we predict user score of (6.86) %.2f' %
            predict2)
      print("Case 2")
      print(predict2)
      assert np.allclose(predict2, 6.85867675)
      print("Case 2 passed!")
      print("\033[92mAll tests passed!")



# Functions to testing multi variable.


def test_gradient_multi(x_train, y_train):
      print("------------test_gradient-----------")
      compute_gradient_test_multi(compute_gradient_multi_obj)


def test_cost_multi(x_train, y_train):
      print("------------test_cost-----------")
      compute_cost_test_multi(cost_test_multi_obj)


def run_gradient_descent_multi(x_train, y_train,alpha = 0.01,iterations=1500,lambda_=0):
      # initialize fitting parameters. Recall that the shape of w is (n,)
      initial_w = np.zeros(x_train.shape[1])
      initial_b = 0.

      print("---- Gradient descent--- lamb ",lambda_)
      lr = LinearRegMulti(x_train,y_train,initial_w,initial_b,lambda_)
      w,b,h,w_init,b_init = lr.gradient_descent(alpha,iterations)

      return w, b



def test_gradient_descent_multi(x_train, y_train):
      w1, b1 = run_gradient_descent_multi(x_train, y_train,0.01,1500,0)
      w1Sol = [0.71606939, 0.08070621, -0.07954303]
      b1Sol = 6.988604092799393
      print(f"w,b found by gradient descent with labmda 0 ({w1Sol} {b1Sol}):", w1, b1)
      numSucces = GetNumGradientsSuccess(w1,w1Sol,b1,b1Sol)

      assert numSucces == (len(w1Sol)+1), f"Case 1: w1,b1 is wrong: {w1},{b1} != {w1Sol},{b1Sol}"

      w2Sol = [ 0.71602344,  0.08071275, -0.07953421]
      b2Sol = 6.988604092799393
      w2, b2 = run_gradient_descent_multi(x_train, y_train,0.01,1500,1)
      print(f"w,b found by gradient descent with labmda 1 ({w2Sol} {b2Sol}):", w2, b2)
      numSucces = GetNumGradientsSuccess(w1,w1Sol,b1,b1Sol)

      assert numSucces == (len(w1Sol)+1), f"Case 2: w2,b2 is wrong: {w2},{b2} != {w2Sol},{b2Sol}"

      print("\033[92mAll tests passed!")


#First Part, Linear Regression
print("First Part, Linear Regression")
#TO-DO the main program.
#test_cost_one(x_train, y_train)
#test_gradient_one(x_train, y_train)
#test_gradient_descent_one(x_train, y_train,w,b)


#Second Part, Linear Regression Multivariable
print("Second Part, Linear Regression Multivariable")
#TO-DO the main program.
#test_cost_multi(x_train, y_train)
#test_gradient_multi(x_train, y_train)
#test_gradient_descent_multi(x_train, y_train)
