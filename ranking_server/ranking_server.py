#!/usr/bin/env python3
# -*- coding: utf-8 -*-

# This is a very basic implementation of a server for storing the ranking records only for development purposes.
# In order to use this server in production, Firestore should be replaced by a more efficient storage and a basic
# authentication system should be added. CORS policy should also be reviewed.

###################
##### IMPORTS #####
###################

import os

import firebase_admin
from firebase_admin import credentials
from firebase_admin import firestore

from flask import Flask
from flask import request, jsonify
from flask_cors import CORS

#####################
##### CONSTANTS #####
#####################

HOST = os.getenv('HOST', "0.0.0.0")
PORT = os.getenv('PORT', 8080)
DEBUG = os.getenv('DEBUG', False)

FIREBASE_CERT_FILE = os.getenv('FIREBASE_CERT_FILE', "serviceAccountKey.json")
FIRESTORE_COLLECTION = os.getenv('FIRESTORE_COLLECTION', "ranking")

#####################
##### VARIABLES #####
#####################

app = Flask(__name__)
cors = CORS(app)

db = None

#####################
##### ENDPOINTS #####
#####################

@app.route("/")
def root():
    return ":)"
    
@app.route("/ranking", methods=["GET"])
def getRanking():
    size = request.args.get('size', default=10, type=int)
    circuit = request.args.get('circuit', default=None)
    
    query = db.collection(FIRESTORE_COLLECTION)
    if circuit is not None:
        query = query.where("circuit", "==", circuit)
    query = query.order_by("time")
    query = query.limit(size)
    
    return jsonify([{
        'id': doc.id,
        **doc.to_dict()
    } for doc in query.stream()]), 200
    
@app.route("/ranking", methods=["POST","PUT"])
def saveRanking():
    name = request.get_json()['name']
    time = request.get_json()['time']
    character = request.get_json()['character']
    circuit = request.get_json()['circuit']
    
    doc_ref = db.collection(FIRESTORE_COLLECTION).document()
    doc_ref.set({
        'name': name,
        'time': time,
        'character': character,
        'circuit': circuit,
        'date': firestore.SERVER_TIMESTAMP
    })
    doc = doc_ref.get()
    
    return jsonify({
        'id': doc.id,
        **doc.to_dict()
    }), 200
    
@app.route("/ranking/<string:id>/position", methods=["GET"])
def getPosition(id):
    circuit = request.args.get('circuit', default=None)
    
    query = db.collection(FIRESTORE_COLLECTION)
    if circuit is not None:
        query = query.where("circuit", "==", circuit)
    query = query.order_by("time")
    
    count = 1
    for doc in query.stream():
        if doc.id == id:
            return jsonify({
                'position': count
            }), 200
        else:
            count += 1
            
    return jsonify({
        'error': f"Not found any record with ID '{id}' for the circuit '{circuit}'"
    }), 404
            
#####################
##### FUNCTIONS #####
#####################

def connectToFirestore():
    cred = credentials.Certificate(FIREBASE_CERT_FILE)
    firebase_admin.initialize_app(cred)
    
    global db
    db = firestore.client()
    
################
##### MAIN #####
################

def main():
    print("Connecting to database ...")
    connectToFirestore()
    print("Launching server ...")
    app.run(host=HOST, port=PORT, debug=DEBUG)

if __name__ == '__main__':
    main()
