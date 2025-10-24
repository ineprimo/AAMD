import numpy as np
import matplotlib.pyplot as plt
from utils import load_data, load_weights,one_hot_encoding, accuracy
from MLP import MLP
from public_test import compute_cost_test, predict_test


x,y = load_data('data/ex3data1.mat')
theta1, theta2 = load_weights('data/ex3weights.mat')

#TO-DO: calculate a testing a prediction and cost.
rainbowdash = MLP(theta1, theta2)
a1,a2,a3,z2,z3 = rainbowdash.feedforward(x)
p = rainbowdash.predict(a3)
#print(p)

# 5 000
print(len(p))

# 50 000
yohe = one_hot_encoding(p)
print(len(yohe))

predict_test(p, y, accuracy)
compute_cost_test(rainbowdash, a3, yohe)