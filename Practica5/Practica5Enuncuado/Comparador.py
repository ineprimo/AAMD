import pandas as pd
import numpy as np
from sklearn.neural_network import MLPClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score
from sklearn.neighbors import KNeighborsClassifier
from sklearn.tree import DecisionTreeClassifier
from sklearn.ensemble import RandomForestClassifier
import MLP_Complete
from sklearn.metrics import ConfusionMatrixDisplay
import matplotlib.pyplot as plt

# --- CONSTANTES ---
CAPAS = (40, 40)
ALPHA = 0.01         # Lambda
LR_INIT = 0.5      
ITERACIONES = 3000 

def main():
    # 1. CARGA DE DATOS
    df = pd.read_csv("preprocessedData.csv")

    # Identificamos columnas de features (X) y target (y)
    target_cols = [col for col in df.columns if col.startswith('action')]
    feature_cols = [col for col in df.columns if col not in target_cols]

    X = df[feature_cols].values
    y_onehot = df[target_cols].values
    
    # Para SKLearn y para calcular métricas, necesitamos las etiquetas como índices (0, 1, 2, 3)
    y_labels = np.argmax(y_onehot, axis=1)

    print(f"Dataset cargado: {X.shape[0]} instancias, {X.shape[1]} características.")

    # 2. SPLIT DE DATOS
    X_train, X_test, y_train_oh, y_test_oh, y_train_lbl, y_test_lbl = train_test_split(
        X, y_onehot, y_labels, test_size=0.3, random_state=42, stratify=y_labels
    )

    # ---------------------------------------------------------
    # 3. MODELO 1: MLP_Complete
    # ---------------------------------------------------------
    print("\n--- Entrenando Custom MLP (Tu implementación) ---")
    
    input_size = X_train.shape[1]
    output_size = y_train_oh.shape[1]
    
    # Instanciamos
    mlp_custom = MLP_Complete.MLP_Complete(input_size, list(CAPAS), output_size, seed=42)
    
    # Entrenamos
    J_history = mlp_custom.backpropagation(
        X_train, 
        y_train_oh, 
        alpha=LR_INIT, 
        lambda_=ALPHA, 
        numIte=ITERACIONES, 
        verbose=100
    )
    
    # Predicción
    a, z = mlp_custom.feedforward(X_test)
    y_pred_custom = mlp_custom.predict(a[-1])
    
    acc_custom = accuracy_score(y_test_lbl, y_pred_custom)

    # ---------------------------------------------------------
    # 4. MODELO 2: MLPClassifier
    # ---------------------------------------------------------
    print("\n--- Entrenando SKLearn MLPClassifier ---")
    
    mlp_sklearn = MLPClassifier(
        hidden_layer_sizes=CAPAS,
        activation='logistic',     
        solver='sgd',             
        max_iter=ITERACIONES,
        learning_rate="adaptive",
        learning_rate_init=LR_INIT,
        n_iter_no_change=50,
        tol=0.0001,
        alpha=ALPHA,             
        random_state=42,
        verbose=100
    )
    
    mlp_sklearn.fit(X_train, y_train_lbl)
    
    y_pred_sklearn = mlp_sklearn.predict(X_test)
    acc_sklearn = accuracy_score(y_test_lbl, y_pred_sklearn)

    """ knn = KNeighborsClassifier(n_neighbors=5)
    knn.fit(X_train, y_train_oh)
    y_pred_knn = knn.predict(X_test)
    acc_knn = accuracy_score(y_test_lbl, y_pred_knn)

    dt = DecisionTreeClassifier(max_depth=10, random_state=42)
    dt.fit(X_train, y_test_oh)
    y_pred_dt = dt.predict(X_test)
    acc_dt = accuracy_score(y_test_lbl, y_pred_dt)

    rf = RandomForestClassifier(n_estimators=100, max_depth=15, random_state=42)
    rf.fit(X_train, y_train_oh)
    y_pred_rf = rf.predict(X_test)
    acc_rf = accuracy_score(y_test_lbl, y_pred_rf) """
    
    print(f"-> Precisión Custom MLP: {acc_custom * 100:.2f}%")
    print(f"-> Precisión SKLearn MLP: {acc_sklearn * 100:.2f}%")
    """print(f"-> Precisión KNN: {acc_knn * 100:.2f}%")
    print(f"-> Precisión DT: {acc_dt * 100:.2f}%")
    print(f"-> Precisión RF: {acc_rf * 100:.2f}%")

    fig, axes = plt.subplots(2, 2, figsize=(15, 12))
    axes = axes.flatten()
    ConfusionMatrixDisplay.from_predictions(y_test_lbl, y_pred_custom, ax=axes[0], cmap="Blues", colorbar=False)
    axes[0].set_title(f"MLP\nAcc: {acc_custom * 100:.2f}%")
    ConfusionMatrixDisplay.from_predictions(y_test_lbl, y_pred_sklearn, ax=axes[1], cmap="Blues", colorbar=False)
    axes[0].set_title(f"SKLearn\nAcc: {acc_sklearn * 100:.2f}%")
    ConfusionMatrixDisplay.from_predictions(y_test_lbl, y_pred_knn, ax=axes[2], cmap="Blues", colorbar=False)
    axes[0].set_title(f"KNN\nAcc: {acc_knn * 100:.2f}%")
    ConfusionMatrixDisplay.from_predictions(y_test_lbl, y_pred_dt, ax=axes[3], cmap="Blues", colorbar=False)
    axes[0].set_title(f"DT\nAcc: {acc_dt * 100:.2f}%")
    ConfusionMatrixDisplay.from_predictions(y_test_lbl, y_pred_rf, ax=axes[4], cmap="Blues", colorbar=False)
    axes[0].set_title(f"RF\nAcc: {acc_rf * 100:.2f}%")"""


if __name__ == "__main__":
    main()