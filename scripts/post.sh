#!/bin/sh

source ./.env

if [ $# -ne 1 ]; then
  echo "実行するにはファイルパスの指定が必要です。" 1>&2
  exit 1
fi

custom_path=$1

docker-compose run --rm blogsync post --title=draft --draft --custom-path=${custom_path} $DOMAIN < draft.md
