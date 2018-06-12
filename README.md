# Stereo-vision tracking of ophthalmic surgical instruments

For background and details:
[Mohamed T. El-Haddad and Yuankai K. Tao, "Automated stereo vision instrument tracking for intraoperative OCT guided anterior segment ophthalmic surgical maneuvers," Biomed. Opt. Express 6, 3014-3031 (2015)](https://www.osapublishing.org/boe/abstract.cfm?uri=boe-6-8-3014)

This code communicates with pixart IR cameras over bluetooth (same cameras used in Wiimotes), and uses the tracking information to control a pair of galvo scanning mirrors using an NI-DAQ. 

Controllers for Thorlabs stages were also integrated for accuracy assessment and coordinate system mapping.

A calibration form is provided which shows what each camera sees. I used a square made up of 4 IR LEDs with known separation to generate stereo calibration files. 
