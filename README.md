# Structure du Projet

Ce dépôt contient trois composants principaux qui fonctionnent ensemble pour créer un système de lancement de jeux d’arcade :

- **ArcadeLauncher** — Interface principale développée avec Unity
- **ArcadeStub** — Exécutable léger en C++ servant de lanceur
- **GamePatcher** — Outil en C# pour nettoyer et préparer les jeux de la vieille version

---

## Autre resources
- [Ajouter Un Jeu](./ADDGAME.md)
- [Exporter](./PACKAGING.md)

---

## Composants du Projet

### 1. `ArcadeLauncher/` (Unity)

> **Type** : Projet Unity  
> **Langage** : C# (Unity)

Il s'agit du **lanceur principal**, développé avec Unity. Il propose :

- Une interface graphique pour sélectionner les jeux
- Une expérience en plein écran adaptée aux bornes d’arcade
- Support des contrôles (clavier, manettes, etc.)
- Affichage d’informations et de métadonnées sur les jeux

---

### 2. `ArcadeStub/` (C++)

> **Type** : Lanceur natif léger  
> **Langage** : C++

`ArcadeStub` est un petit exécutable natif en C++ dont le rôle est de :

- Lancer `ArcadeStub.exe`
- Fournir une couche d’abstraction ou de compatibilité, si nécessaire
- Être placé **à la racine du dossier de build** sous le nom `ArcadeStub.exe`

- ### 3. `GamePatcher/` (C#)

> **Type** : Outil de patch / nettoyage de jeux  
> **Langage** : C#

---

`GamePatcher` est un outil de ligne de commande permettant de préparer et nettoyer les jeux avant leur intégration dans le système.

**Fonctionnalités principales :**

- Injection de code qui empèche les jeux faites pour le vieux Portail de ne pas quitter
