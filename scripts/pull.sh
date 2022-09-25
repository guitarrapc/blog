#!/bin/sh

source ./.env

docker-compose run --rm blogsync pull $DOMAIN