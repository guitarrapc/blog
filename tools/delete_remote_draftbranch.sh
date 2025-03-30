#!/bin/bash
set -eo pipefail

repo=guitarrapc/blog
for branch in $(gh api "repos/$repo/branches" --jq '.[].name' | grep '^draft-entry-' | grep -v "draft-entry-6802418398339652746"); do
  echo "Deleting remote branch: $branch"
  gh api --method DELETE -H "Accept: application/vnd.github+json" "/repos/$repo/git/refs/heads/$branch"
done
