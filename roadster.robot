*** Test Cases ***
Should Run VMS Original Image
    Execute Command           include @${CURDIR}/roadster.resc
    Create Terminal Tester    sysbus.canuart
    Create Log Tester         10

    Wait For Line On Uart     LPC2xxx I2C Driver
    Wait For Line On Uart     checktime

    # VMS should start automatically
    Wait For Line On Uart     VMS restarted - version 15.6.0
    Wait For Prompt On Uart   ?

    # Wait for the last unique decoded message we see
    Wait For Log Entry        decoder: SWP variable device state report interval set

