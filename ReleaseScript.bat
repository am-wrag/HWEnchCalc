erase *.application *.vshost.exe *.manifest *.json app.publish
erase *.pdb
erase *.xml
md Data
for %%f in (*.dll) do move %%f Data
erase ReleaseScript.bat