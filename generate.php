<?php
// Database connection parameters
$host = 'localhost'; // Replace with your database host
$db = 'hr_cam'; // Replace with your database name
$user = 'lia'; // Replace with your database username
$pass = 'lia'; // Replace with your database password

// Create a new PDO instance for database connection
try {
    $pdo = new PDO("mysql:host=$host;dbname=$db;charset=utf8", $user, $pass);
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
} catch (PDOException $e) {
    echo 'Connection failed: ' . $e->getMessage();
    exit;
}

// Function to generate a random string
function generateRandomString($length = 10)
{
    $characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
    $charactersLength = strlen($characters);
    $randomString = '';
    for ($i = 0; $i < $length; $i++) {
        $randomString .= $characters[rand(0, $charactersLength - 1)];
    }
    return $randomString;
}

// Prepare SQL statement with placeholders
$sql = "INSERT INTO `persons` (`type`, `identification_number`, `name`, `gender`, `image_file`, `notes`, `reference`, `is_approved`, `is_synced`, `is_updated`, `is_deleted`, `created_at`, `updated_at`) 
        VALUES ('employee', :identification_number, :name, 'unknown', '1687b5cc-ba7a-4838-92b6-04de79c70360.jpg', NULL, NULL, '1', '0', '0', '0', current_timestamp(), current_timestamp())";

// Begin transaction
try {
    $pdo->beginTransaction();

    $stmt = $pdo->prepare($sql);

    // Loop to insert 1000 records
    for ($i = 1; $i <= 3000; $i++) {
        $customId = "coba" . $i; // Example custom ID, you might want to adjust this logic
        $randomIdentificationNumber = generateRandomString(8); // Generate a random identification number
        $randomName = generateRandomString(5); // Generate a random name

        $stmt->execute([
            ':identification_number' => $customId,
            ':name' => $randomName
        ]);

        if ($i % 100 === 0) {
            // Commit every 100 records to avoid holding the transaction open for too long
            $pdo->commit();
            $pdo->beginTransaction();
        }
    }

    // Commit any remaining records
    $pdo->commit();

    echo "1000 records inserted successfully.";
} catch (PDOException $e) {
    // Roll back the transaction in case of error
    $pdo->rollBack();
    echo 'Query failed: ' . $e->getMessage();
}
?>