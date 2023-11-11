# AP_GameDev_Project
## Room Files
The first 2 bytes show the width of the room expressed in binary. The width of the room is defined as the amount of tiles it would take to go from the left side to the right side. For example '0x00 0x05' would display a width of 5 tiles. The next 2 bytes is the spawnpoint index. The rest of the bytes of the file can be interpreted as the **tiletype** index. '0x00' is defined as no tile and will be left blank. Room files must be put in the directory 'Rooms/' and contain the '.room' file extension. The file must be given the property 'Copy to Output Directory: Copy always'. The bytes are stored using Big-Endian.

For example:
```0005 005c 0101 0101 0101 0000 0001 0100 0000 0101 0000 0001 0101 0101 01```  
Will output a room of 5 tiled wide that looks like this:  
  
![image](https://github.com/SkyPromp/AP_GameDev_Project/assets/44906497/e6f9ee5f-e0c6-4c65-bfc5-f2e38a5e8473)
