import socket

class Client():

    # Variables
    IP = "localhost"
    PORT = 6789
    MESSAGE = ""
    ANSWER = ""

    # Prepares and connects to socket
    tcpClient = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    tcpClient.connect((IP, PORT))

    # Loop keep the client connected
    while True:

        # For loop to allow user to send 5 messages
        # Prints the answer from the server
        for _ in range(5):
            MESSAGE = input("\nType message to server: ")
            # \n and .encode("UTF-8") is VERY IMPORTANT!
            tcpClient.send((MESSAGE + "\n").encode("UTF-8"))
    
            ANSWER = tcpClient.recv(4096)
            print("Server answer:", ANSWER)
        