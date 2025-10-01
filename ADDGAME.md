# Ajouter un nouveau jeu
## Fonctionnement générale
L'executable **ArcadeLauncher** va automatiquement chercher le répertoire ```Games``` pour des jeux.
Tout ce qui faut pour que le jeu soit reconnu, c'est qu'il contienent à la base du jeu, un fichier ```manifest.json```.

Malgré que le fichier ```manifest.json``` doit être à la base du répertoire, l'executable du jeu ne doit pas nécéssairement l'être.

### Example de structure de jeux:
```
ExampleGame/
├─ D3D12/
│  └─ ...
├─ MonoBleedingEdge/
│  └─ ...
├─ ExampleGame_Data/
│  └─ ...
├─ ExampleGame.exe
├─ UnityCrashHandler.exe
├─ UnityPlayer.dll
└─ manifest.json
```
## Créer un fichier manifest
La structure du manifest est quand même asser simple.
Tout ce qu'il contient sont les info requis pour lancer et jeu, ainsi que des données pour afficher l'auteur, version, image, etc...

### Explications
- **Manifest Version**: La version de la structure de données du fichier (actuellement il n'y a que 1)+
- **Game Path**: Le chemin ou se trouve l'executable du jeu de la base de l'executable ArcadeLauncher
- **Game Name**: Le nom du jeu qui sera montré dans le launcher
- **Game Version**: La version du jeu
- **Author**: Le ou les auteurs qui ont créé le jeu
- **Image Path**: Le chemin de l'image de la base de l'executable ArcadeLauncher qui sera montré dans le launcher

### Example d'un fichier manifest
```json
{
  "manifestVersion": 1,
  "gamePath": "Games/ExampleGame.exe",
  "gameName": "Example Game",
  "gameVersion": "1.0.0",
  "author": "Toi",
  "imagePath": "Images/ExampleGame.png"
}
```
