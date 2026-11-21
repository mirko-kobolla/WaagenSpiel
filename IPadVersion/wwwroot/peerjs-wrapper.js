window.waagenPeer = {
    peer: null,
    hostConnection: null,
    connections: new Map(),
    dotnet: null,

    async createHost(dotnetReference) {
        this.dotnet = dotnetReference;
        const roomCode = this.createRoomCode();
        this.roomCode = roomCode;
        this.peer = new Peer(roomCode);

        this.peer.on('open', peerId => {
            dotnetReference.invokeMethodAsync('OnPeerEvent', JSON.stringify({
                type: 'host-ready',
                peerId: peerId.toUpperCase()
            }));
        });

        this.peer.on('connection', connection => {
            connection.on('open', () => {
                this.connections.set(connection.peer, connection);
            });

            connection.on('data', data => {
                dotnetReference.invokeMethodAsync('OnPeerEvent', JSON.stringify({
                    type: 'peer-message',
                    peerId: connection.peer,
                    payload: this.parsePayload(data)
                }));
            });

            connection.on('close', () => {
                this.connections.delete(connection.peer);
                dotnetReference.invokeMethodAsync('OnPeerEvent', JSON.stringify({
                    type: 'peer-disconnected',
                    peerId: connection.peer
                }));
            });
        });

        this.peer.on('error', error => {
            dotnetReference.invokeMethodAsync('OnPeerEvent', JSON.stringify({
                type: 'peer-error',
                message: error?.message ?? String(error)
            }));
        });
    },

    async joinRoom(hostId, dotnetReference) {
        this.dotnet = dotnetReference;
        this.peer = new Peer();

        this.peer.on('open', () => {
            this.hostConnection = this.peer.connect(hostId.trim().toUpperCase(), { reliable: true });

            this.hostConnection.on('open', () => {
                dotnetReference.invokeMethodAsync('OnPeerEvent', JSON.stringify({
                    type: 'joined-ready',
                    peerId: this.peer.id
                }));
            });

            this.hostConnection.on('data', data => {
                dotnetReference.invokeMethodAsync('OnPeerEvent', JSON.stringify({
                    type: 'host-message',
                    payload: this.parsePayload(data)
                }));
            });

            this.hostConnection.on('close', () => {
                dotnetReference.invokeMethodAsync('OnPeerEvent', JSON.stringify({
                    type: 'host-disconnected'
                }));
            });
        });

        this.peer.on('error', error => {
            dotnetReference.invokeMethodAsync('OnPeerEvent', JSON.stringify({
                type: 'peer-error',
                message: error?.message ?? String(error)
            }));
        });
    },

    sendToHost(payload) {
        if (this.hostConnection?.open) {
            this.hostConnection.send(payload);
        }
    },

    sendToPeer(peerId, payload) {
        const connection = this.connections.get(peerId);
        if (connection?.open) {
            connection.send(payload);
        }
    },

    broadcast(payload) {
        for (const connection of this.connections.values()) {
            if (connection.open) {
                connection.send(payload);
            }
        }
    },

    async close() {
        this.connections.clear();
        this.hostConnection = null;
        if (this.peer) {
            this.peer.destroy();
            this.peer = null;
        }
    },

    parsePayload(payload) {
        if (typeof payload !== 'string') {
            return payload;
        }

        try {
            return JSON.parse(payload);
        } catch {
            return payload;
        }
    },

    createRoomCode() {
        const alphabet = 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789';
        const randomValues = new Uint32Array(6);
        crypto.getRandomValues(randomValues);

        return Array.from(randomValues, value => alphabet[value % alphabet.length]).join('');
    }
};
