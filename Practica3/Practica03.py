import numpy as np
import matplotlib.pyplot as plt
from utils import load_data, load_weights,one_hot_encoding, accuracy, displayData
from MLP import MLP
from public_test import compute_cost_test, predict_test
from sklearn.metrics import confusion_matrix


x,y = load_data('data/ex3data1.mat')
theta1, theta2 = load_weights('data/ex3weights.mat')

# 1
rainbowdash = MLP(theta1, theta2)
a1,a2,a3,z2,z3 = rainbowdash.feedforward(x)
p = rainbowdash.predict(a3)
predict_test(p, y, accuracy)

# 2
yohe = one_hot_encoding(y)
compute_cost_test(rainbowdash, a3, yohe)

# 3
y_b = (y == 0).astype(int)
p_b = (p == 0).astype(int)

cm = confusion_matrix(y_b, p_b)
tn, fp, fn, tp = cm.ravel().tolist()

# precission
precision = tp/(tp+fp)
# recall
recall = tp/(tp + fn)
# f1
f1 = 2 * (precision*recall)/(precision + recall)

print(cm)
print(precision)
print(recall)
print(f1)
