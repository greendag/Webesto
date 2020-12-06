#!/bin/bash

function runCmd() {
    # Sometimes the commands fail the first time - so this function runs them again if necessary
    for i in {0..1}; do
      $1 $2 $3
      [ $? -eq 0 ] && break
    done
}

if [ $# -ne 2 ]; then
  echo "Executing command with params: $1 $2"
  echo "Wrong number of arguments, got $# expected 2"
  echo ''
  echo 'Usage:  publish.sh <raspberry pi hostname> <runtime>'
  echo ''
  echo 'Example 1: publish.sh raspberrypi linux-arm'
else
  RASPBERRYPI_HOSTNAME=$1
  RUNTIME=$2

  SOLUTION_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && cd .. && pwd )"

  SOLUTION_PATH=$(find /c/Users/gary/Documents/Projects/Webesto -name "*.sln")

  SOLUTION_FILE=$(basename $SOLUTION_PATH)

  SOLUTION_NAME=${SOLUTION_FILE%.*}

  OUTPUT_DIR="$SOLUTION_DIR/bin/$RUNTIME/Debug"

  echo "Publishing solution $SOLUTION_FILE to $RASPBERRYPI_HOSTNAME with a runtime of $RUNTIME"
  dotnet publish $SOLUTION_FILE --output $OUTPUT_DIR --runtime $RUNTIME --framework net5.0 --configuration Debug --force --self-contained false >/dev/null
  exitcode=$?
  if [ $exitcode -ne 0 ]; then
    echo "Publish failed, exit code $exitcode... exiting"
    exit $exitcode
  fi

  EXECUTABLE_PATH=$(find $OUTPUT_DIR -type f ! -name "*.*")

  EXECUTABLE_FILE=$(basename $EXECUTABLE_PATH)

  echo "Creating remote directory $RASPBERRYPI_HOSTNAME:/home/pi/Debug/$SOLUTION_NAME"
  runCmd ssh pi@$RASPBERRYPI_HOSTNAME "mkdir ~/Debug/$SOLUTION_NAME -p" >/dev/null
  exitcode=$?
  if [ $exitcode -ne 0 ]; then
    echo "Failed to create the directory $RASPBERRYPI_HOSTNAME:/home/pi/Debug/$SOLUTION_NAME. exit code $exitcode... exiting"
    exit $exitcode
  fi

  echo "Copying published files to remote system $RASPBERRYPI_HOSTNAME"
  runCmd scp "-r $OUTPUT_DIR/*" pi@$RASPBERRYPI_HOSTNAME:Debug/$SOLUTION_NAME >/dev/null
  exitcode=$?
  if [ $exitcode -ne 0 ]; then
    echo "Failed to copy files from $OUTPUT_DIR to $RASPBERRYPI_HOSTNAME:/home/pi/Debug/$SOLUTION_NAME. exit code $exitcode... exiting"
    exit $exitcode
  fi

#  echo "Making the remote file $RASPBERRYPI_HOSTNAME:/home/pi/Debug/$SOLUTION_NAME/$EXECUTABLE_FILE executable"
#  runCmd ssh pi@$RASPBERRYPI_HOSTNAME "chmod 755 ~/Debug/$SOLUTION_NAME/$EXECUTABLE_FILE" >/dev/null
#  exitcode=$?
#  if [ $exitcode -ne 0 ]; then
#    echo "Failed to make the file $RASPBERRYPI_HOSTNAME:/home/pi/Debug/$SOLUTION_NAME/$EXECUTABLE_FILE executable. exit code $exitcode... exiting"
#    exit $exitcode
#  fi
fi
