# Instructions de Packaging

Ce projet est développé avec **Unity**. Ce document décrit comment générer et packager correctement le build de **ArcadeLauncher**, prêt pour distribution.

---

## Prérequis

- Unity installé (version utilisée : spécifiez ici si nécessaire).
- Le projet Unity ouvert et fonctionnel.
- `LauncherStub.exe` (fourni séparément).

---

## Étapes de Build & Packaging

### 1. Ouvrir le projet dans Unity

Lancez Unity Hub, ouvrez le projet **ArcadeLauncher**, puis vérifiez que la scène de démarrage est bien configurée dans les **Build Settings**.

### 2. Configurer les Build Settings

- Allez dans `File > Build Settings`
- Ciblez la plateforme souhaitée (ex. : **Windows**)
- Ajoutez les scènes nécessaires dans le champ "Scenes In Build"
- Cliquez sur **Player Settings** pour configurer le nom, l’icône, la résolution, etc., si nécessaire

### 3. Générer le build

Dans la fenêtre **Build Settings** :

- Cliquez sur **Build**
- Choisissez un dossier de sortie, par exemple :

- Unity va générer un dossier contenant :
- `ArcadeLauncher.exe`
- `ArcadeLauncher_Data/`
- Et d’autres fichiers nécessaires

### 4. Ajouter `LauncherStub.exe`

Copiez **manuellement** le fichier `LauncherStub.exe` à la **racine du dossier de build** :

> **Important** : `LauncherStub.exe` n’est pas généré par Unity. Il doit être compilé et copié **manuellement** après chaque build.

---

## Tester le build

- Double-cliquez sur `ArcadeLauncher.exe` pour lancer le jeu.
- Vérifiez que tout fonctionne correctement (résolution, scènes, assets...).

---
