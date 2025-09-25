import numpy as np

def compute_cost_test_one(target):
	print("Testing: compute_cost_test")
	# print("Using X with shape (4, 1)")
	# Case 1
	x = np.array([2, 4, 6, 8]).T
	y = np.array([7, 11, 15, 19]).T
	initial_w = 2
	initial_b = 3.0
	cost = target(x, y, initial_w, initial_b)
	print("Cost case 1")
	assert cost == 0, f"Case 1: Cost must be 0 for a perfect prediction but got {cost}"
	print("Case 1 passed!")
	# Case 2
	x = np.array([2, 4, 6, 8]).T
	y = np.array([7, 11, 15, 19]).T
	initial_w = 2.0
	initial_b = 1.0
	cost = target(x, y, initial_w, initial_b)
	print("Cost case 2")
	assert cost == 2, f"Case 2: Cost must be 2 but got {cost}"
	print("Case 2 passed!")
	# print("Using X with shape (5, 1)")
	# Case 3
	x = np.array([1.5, 2.5, 3.5, 4.5, 1.5]).T
	y = np.array([4, 7, 10, 13, 5]).T
	initial_w = 1
	initial_b = 0.0
	cost = target(x, y, initial_w, initial_b)
	print("Cost case 3")
	assert np.isclose(cost, 15.325), f"Case 3: Cost must be 15.325 for a perfect prediction but got {cost}"
	print("Case 3 passed!")
	# Case 4
	initial_b = 1.0
	cost = target(x, y, initial_w, initial_b)
	print("Cost case 4")
	assert np.isclose(cost, 10.725), f"Case 4: Cost must be 10.725 but got {cost}"
	print("Case 4 passed!")
	# Case 5
	y = y - 2
	initial_b = 1.0
	cost = target(x, y, initial_w, initial_b)
	print("Cost case 5")
	assert  np.isclose(cost, 4.525), f"Case 5: Cost must be 4.525 but got {cost}"
	print("Case 5 passed!")
	print("\033[92mAll tests passed!")
	
def compute_gradient_test_one(target):
	print("Testing: compute_gradient_test")
	print("Using X with shape (4, 1)")
	# Case 1
	x = np.array([2, 4, 6, 8]).T
	y = np.array([4.5, 8.5, 12.5, 16.5]).T
	initial_w = 2.
	initial_b = 0.5
	dj_dw, dj_db = target(x, y, initial_w, initial_b)
	print("Case 1")
	print(dj_dw,dj_db)
	#assert dj_dw.shape == initial_w.shape, f"Wrong shape for dj_dw. {dj_dw} != {initial_w.shape}"
	assert dj_db == 0.0, f"Case 1: dj_db is wrong: {dj_db} != 0.0"
	assert np.allclose(dj_dw, 0), f"Case 1: dj_dw is wrong: {dj_dw} != [[0.0]]"
	print("Case 1 passed!")
	# Case 2 
	x = np.array([2, 4, 6, 8]).T
	y = np.array([4, 7, 10, 13]).T + 2
	initial_w = 1.5
	initial_b = 1
	dj_dw, dj_db = target(x, y, initial_w, initial_b)
	print("Case 2")
	#assert dj_dw.shape == initial_w.shape, f"Wrong shape for dj_dw. {dj_dw} != {initial_w.shape}"
	assert dj_db == -2, f"Case 1: dj_db is wrong: {dj_db} != -2"
	assert np.allclose(dj_dw, -10.0), f"Case 1: dj_dw is wrong: {dj_dw} != -10.0"   
	print("Case 2 passed!")
	print("\033[92mAll tests passed!")
	

def test_data_multi():
    X_train = np.array([[2104, 5, 1, 45], [1416, 3, 2, 40], [852, 2, 1, 35]])
    y_train = np.array([460, 232, 178])

    b_init = 785.1811367994083
    w_init = np.array([0.39133535, 18.75376741, -53.36032453, -26.42131618])

    return X_train, y_train, w_init, b_init


def compute_cost_test_multi(target):
    X_train, y_train, w_init, b_init = test_data_multi()
    cost = target(X_train, y_train, w_init, b_init)
    target_cost = 1.5578904045996674e-12
    assert np.isclose(
        cost, target_cost, rtol=1e-4), f"Case 1: Cost must be {target_cost} for a perfect prediction but got {cost}"

    print("\033[92mAll tests passed!")


def compute_gradient_test_multi(target):
    X_train, y_train, w_init, b_init = test_data_multi()

    dj_dw, dj_db = target(X_train, y_train, w_init, b_init)
    #assert dj_dw.shape == w_init.shape, f"Wrong shape for dj_dw. {dj_dw} != {w_init.shape}"
    target_dj_db = -1.6739251122999121e-06
    target_dj_dw = [-2.73e-3, - 6.27e-6, - 2.22e-6, - 6.92e-5]

    assert np.isclose(dj_db, target_dj_db,
                      rtol=1e-4), f"Case 1: dj_db is wrong: {dj_db} != {target_dj_db}"
    assert np.allclose(
        dj_dw, target_dj_dw, rtol=1e-02), f"Case 1: dj_dw is wrong: {dj_dw} != {target_dj_dw}"

    print("\033[92mAll tests passed!")