# No-More-League
A service that deletes League of Legends that cannot be removed normally.

How to install: 
- Compile or download a release and run this in admin powershell.
- Make sure to replace "Path_to_the_exe_file" with the path to the executable.
```
New-Service -Name "No More League" -BinaryPathName Path_to_the_exe_file
```

<details> 
  <summary>How to remove? [Don't click unless you want to lose your sanity] </summary>
  1st way: Run Windows in safe mode and delete the executable file.
  
  2nd way: Run this in admin powershell:
  ```Remove-Service -Name "No More League"```
  then this: ```sc.exe delete "No More League"```
</details>
