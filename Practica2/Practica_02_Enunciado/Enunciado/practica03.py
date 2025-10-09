from public_tests_logistic_multi import compute_cost_test, compute_gradient_test
from utils import load_data_csv_multi_logistic,zscore_normalize_features

from LogisticRegressionMulti import LogisticRegMulti
from LogisticRegressionMulti import cost_test_multi_obj
from LogisticRegressionMulti import compute_gradient_multi_obj
import pandas as pd
import numpy as np


def test_gradient(x_train, y_train):
    print("------------test_gradient-----------")
    compute_gradient_test(compute_gradient_multi_obj)


def test_cost(x_train, y_train):
     print("------------test_cost-----------")
     compute_cost_test(cost_test_multi_obj)


def run_gradient_descent(x_train, y_train,alpha = 0.01,iterations=1500,lambda_=0):
    # initialize fitting parameters. Recall that the shape of w is (n,)
    initial_w = np.zeros(x_train.shape[1])
    initial_b = 0.

    print("---- Gradient descent POO--- lamb ",lambda_)
    lr = LogisticRegMulti(x_train,y_train,initial_w,initial_b,lambda_)
    w,b,h,w_init,b_init = lr.gradient_descent(alpha,iterations)
    
    return w, b



def test_gradient_descent(x_train, y_train):
    #print("----- Original Gradient descent------")
    w1, b1 = run_gradient_descent(x_train, y_train,0.01,1500,0)
    print("w,b found by gradient descent with labmda 0 ([ 0.93305656  0.18903186 -0.12087087] 0.4690649858291144):", w1, b1)

    w2, b2 = run_gradient_descent(x_train, y_train,0.01,1500,1)
    print("w,b found by gradient descent with labmda 1 ([ 0.93278101  0.18900175 -0.12080013] 0.46905464164492655):", w2, b2)



x_train, y_train = load_data_csv_multi_logistic("./data/games-data.csv","score","critics","users","user score")
x_train, mu, sigma = zscore_normalize_features(x_train)
test_cost(x_train, y_train)
test_gradient(x_train, y_train)
test_gradient_descent(x_train, y_train)
