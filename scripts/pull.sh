#!/bin/bash

set -eu

source ./.env
docker-compose run --rm blogsync pull "$DOMAIN"
