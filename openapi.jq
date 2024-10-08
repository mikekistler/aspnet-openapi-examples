# jq module for OpenAPI data types

# backport of pick from jq 1.7
def pick(pathexps):
  . as $in
  | reduce path(pathexps) as $a (null;
      setpath($a; $in|getpath($a)) );

def show_schemas(schemas):
    .components.schemas
    | pick(schemas)
    | {"schema": .}
;
