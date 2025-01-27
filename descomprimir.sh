#!/bin/bash

# Asegúrate de que el archivo rar esté presente
if [ ! -f "$1" ]; then
  echo "El archivo $1 no existe. Abortando."
  exit 1
fi

# Crear carpeta donde descomprimir los archivos
mkdir -p descomprimido

# Descomprimir el archivo .rar o los archivos divididos
echo "Descomprimiendo $1..."
unrar x "$1" descomprimido/

# Verificar si la descompresión fue exitosa
if [ $? -eq 0 ]; then
  echo "Descompresión exitosa."
else
  echo "Error en la descompresión."
  exit 1
fi
