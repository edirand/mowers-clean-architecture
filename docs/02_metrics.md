# Available metrics

## API

| metric| type | description |
| -- | -- | -- |
|http_server_duration_ms | Histogram | Records http server requests duration |
| | |

## Worker

| metric| type | description |
| -- | -- | -- |
|worker_file_processing_counter | Counter | A counter, incremented each time a file is processed by the worker |
| worker_file_processing_duration_ms | Histogram| Records processing time for each file processed by the worker. This metric uses *success* tag to separate errors from succes.|
|worker_file_processing_error_counter | Counter | A counter, incremented each time the processing of a file failed |
| | |