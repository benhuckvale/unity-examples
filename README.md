Unity-examples
==============

This project assumes no familiarity with Unity whatsoever. The objective is to
have examples that run out of the box and run programmatically without needing
to interact manually with the Unity Editor to manipulate assets or modify settings.
Fully automating a build is the foundation of a continuous integration process
so I suppose I am aiming this project to be a starting point for an application
that considers automation from the outset, or as much as possible.


Prerequisites
-------------

### OS X

Run:
```
brew install unity
```

This asks for admin privileges as it will put Unity and Unity Hub into the /Applications directory.

The result of installation is that you now have this executable:
```
/Applications/Unity/Unity.app/Contents/MacOS/Unity
```

In the rest of this README.md, we will just refer to this as `Unity`. The
CMakeLists.txt defines this full path and uses it. If using it outside cmake
then obviously add it to your PATH or alias it for convenience. 

To actually build anything you will need to [obtain a
license](#obtaining-a-license). You can select a free personal one if you are
just an individual playing around.

Unity supports output to many different contexts. The WebGL context is pretty
neat as it means you can create a game you can play through a web browser.

To be able to build to this context, which is what this project is set up to
do, we need to:

```
brew install unity-webgl-support-for-editor
```


Build
-----

Run:
```
cmake -B build -S .
```

to configure the build. In the usual cmake philosophy this creates an
out-of-source build in the directory 'build/'. The conventional Unity project
folders are to be found in there, some of which are copied from the source
directory `.`. If the Unity Editor is to be opened manually, this is the
project folder to open.

Then
```
cmake --build build --target webgl
```


Run
---

To actually run the project the server/ directory is used.

Node.js needs to be installed, which is most easily done via [Node Version
Manager (NVM)](https://github.com/nvm-sh/nvm) which gives you the option to try
alternative versions.

You also need a self-signed cert in it, which you can create with OpenSSL like this:
```
cd server/
openssl genpkey -algorithm RSA -out private.key
openssl req -new -key private.key -x509 -days 365 -out certificate.crt -subj "/C=GB"
cd ..
```
Or with a different "/C=<country>" or otherwise as you see fit. The country is the minimum
you need in the subject field.

With node installed, you can:

```
cd server/
npm install
node server.js ../build/Builds/WebGL
```

Browse to https://localhost:8080/. The first time you will need to accept the
connection warning and "Proceed to localhost (unsafe)" unless you do not trust
your own certificate. It is not usable yet - you will see the error showing
that the server needs to serve files with certain headers present.


Obtaining a license
-------------------

You need to login to the unity website and create a unity id, and then to
obtain and activate a license one follows [the manual activation
guide](https://docs.unity3d.com/Manual/ManualActivationCmdWin.html)

Essentially that is to do the following

### OS X

1. Run
```
Unity -batchmode -createManualActivationFile -logfile
```

Which will create a file like this:
```
Unity_v2022.1.23f1.alf
```

2. Browse to https://license.unity3d.com/manual
and upload that file, selecting appropriate license etc.
Then the license file can be downloaded. It will be something like:
```
Unity_v2022.x.ulf
```

3. Then you activate that license:
```
/Applications/Unity/Unity.app/Contents/MacOS/Unity -batchmode -manualLicenseFile Unity_v2022.x.ulf -logfile
```

These commands can be executed anywhere as they are to do with configuring the
installation of Unity and have nothing to do with this project (except that
this project needs a licensed Unity executable it can use).

