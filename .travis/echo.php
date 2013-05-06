<?php

$status = (int) $_GET['_status'];
unset($_GET['_status']);

header('HTTP/1.1 ' . $status, true, $status);
header('Content-Type: text/plain');

echo json_encode($_GET);
