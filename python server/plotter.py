import random
import math 
import time
import json

class Plotter():
    #After these steps one 2 Pi Cycle is done
    cycle = 20
    #Sin Cycle repeats after these hours
    hourcycle = 3
    #Number of wanted Values
    steps = 100
    #The Value the Numbers should middle around
    middleValue = 10
    #How Height and Low should the sin function go
    maxSinHeight = 5
    #Random from picked Value
    maxRandDifference = 1
    #Precission of rand steps
    randSteps = 0.1
    #To what decimal places should be round?
    rounding = 2

    randInt = int(maxRandDifference/randSteps)

    def getValue(self, seed, timestamp):
        hour = timestamp/3600
        randInt = int(self.maxRandDifference/self.randSteps)
        sinSteps = (2*math.pi)/self.hourcycle

        sinValue = math.sin(sinSteps*hour)
        sinValue = sinValue * self.maxSinHeight
        random.seed(seed)
        sinValue = sinValue + random.randrange(-randInt, randInt) * self.randSteps
        sinValue = sinValue + self.middleValue
        sinValue = round(sinValue, self.rounding)

        resultString = str(sinValue)

        print(resultString)

        return resultString

    def getJson(self, timestamp):

        #Every these Minutes a new Value is given
        stepMinutes = 1
        stepSeconds = stepMinutes*60
        currentTimestamp = int(time.time())

        timestamp = int(timestamp/stepSeconds) + 1

        dataSet = []

        while timestamp*stepSeconds < currentTimestamp:
            
            dataSet.append({
                'timestamp' : timestamp*stepSeconds,
                'value' : self.getValue(timestamp, timestamp*stepSeconds)
            })

            timestamp += 1

        return json.dumps(dataSet)


    def writeTxt(self):
        sinSteps = (2*math.pi)/self.cycle

        open('plotted.txt', 'w').close()

        with open('plotted.txt', 'w') as f:
            for i in range(1, self.steps+1):

                sinValue = math.sin(sinSteps*i)
                sinValue = sinValue * self.maxSinHeight
                sinValue = sinValue + random.randrange(-self.randInt, self.randInt) * self.randSteps
                sinValue = sinValue + self.middleValue
                sinValue = round(sinValue, self.rounding)

                resultString = str(sinValue)

                print(resultString, file=f)
        
       



