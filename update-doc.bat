:: IMPORTANT copy this file to the directory ../Onyx, then execute .bat. 
::file is here to put it in the repo git
::but it cannot execute here, you need to copy this file a folder before and then execute it. 

:: reminder : update the INDEX

ECHO OFF
ECHO ##############################  Update doc in step branches  ##############################
::PAUSE

SET INDEX=10

::SAVE MASTER DOCUMENTATION TO TEMPO DIR, OUTSIDE GIT REPO
cd Onyx
git stash
git checkout -f master
cd ..
rmdir /S /Q "tempo"
mkdir "tempo"
xcopy /Q /Y /I "Onyx\README.md" tempo\  
xcopy /K /S /Y /I /Q "Onyx/docs" "tempo/docs/*" 

FOR /L %%A IN (1,1,%INDEX%) DO (
    ECHO ############################## Working on branch : step%%A
    
    cd Onyx
    git checkout -f step%%A
    cd ..

    xcopy /Y /I /Q "tempo\README.md" Onyx\ 
    xcopy /K /S /Y /I /Q  "tempo/docs" "Onyx/docs/*"

    cd Onyx
    git sl
    git stash
    git add .
    git commit -m "doc update"
    git push
    git checkout master
    cd ..
)

PAUSE
