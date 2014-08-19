# uTorrent Auto Labeller (uAL)

This console application automatically loads and labels torrents (based on subfolder names) from the uTorrent "watch folder".

## Setup
 1. Start uTorrent and enable Remote Access (Options > Preferences > Remote)
 2. Copy the uAL > bin > Release folder to the directory you wish to run this program from (E.g. C:\Program Files\uAL)
 3. Run uAL.exe
 4. Answer the questions to complete set up (first time only) and leave the application running as long as uTorrent is running.

> Note:
> - Restarting uTorrent means restarting this application
> - If the IP is different from "localhost" or "127.0.0.1", the user must also input a local directory for the app to look for torrents in.
> - *Delete torrents* option doesn't actually delete the torrents rather it simply moves them to the Windows Temp folder.

## Folders
E.g. With the following folder structure:

```
+-- WatchFolder
|   +-- SubFolder1
```
Placing a .torrent file in *SubFolder1* will automatically load the torrent and set the label as "*SubFolder1*"

## Example Usage
Set up an HTPC with a cloud-synced folder (like Dropbox) as the uTorrent watch folder and add torrent files remotely to subfolders to have them auto-loaded & labelled.


## References:
 1. [Forum Discussion](http://forum.utorrent.com/topic/71350-utorrent-auto-labeller/)
 2. [uTorrent Web API](http://www.utorrent.com/community/developers/webapi)