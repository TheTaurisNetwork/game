// Torque Input Map File
if (isObject(moveMap)) moveMap.delete();
new ActionMap(moveMap);
moveMap.bindCmd(keyboard, "escape", "", "handleEscape();");
moveMap.bind(keyboard, "a", moveleft);
moveMap.bind(keyboard, "d", moveright);
moveMap.bind(keyboard, "left", moveleft);
moveMap.bind(keyboard, "right", moveright);
moveMap.bind(keyboard, "w", moveForward);
moveMap.bind(keyboard, "s", movebackward);
moveMap.bind(keyboard, "up", moveForward);
moveMap.bind(keyboard, "down", movebackward);
moveMap.bind(keyboard, "e", interactObject);
moveMap.bind(keyboard, "c", movedown);
moveMap.bind(keyboard, "space", JUMP);
moveMap.bind(keyboard, "u", toggleMessageHud);
moveMap.bind(keyboard, "t", teamMessageHud);
moveMap.bind(keyboard, "pageup", pageMessageHudUp);
moveMap.bind(keyboard, "pagedown", pageMessageHudDown);
moveMap.bind(keyboard, "o", resizeMessageHud);
moveMap.bind(keyboard, "tab", toggleFirstPerson);
moveMap.bind(keyboard, "alt c", toggleCamera);
moveMap.bind(keyboard, "f3", startRecordingDemo);
moveMap.bind(keyboard, "f4", stopRecordingDemo);
moveMap.bind(keyboard, "f8", dropCameraAtPlayer);
moveMap.bind(keyboard, "f7", dropPlayerAtCamera);
moveMap.bind(keyboard, "q", mediPack);
moveMap.bind(keyboard, "l", toggleFlashlight);
moveMap.bind(keyboard, "numpadenter", showinventory);
moveMap.bind(keyboard, "numpad1", selectFavorite1);
moveMap.bind(keyboard, "numpad2", selectFavorite2);
moveMap.bind(keyboard, "numpad3", selectFavorite3);
moveMap.bind(keyboard, "numpad4", selectFavorite4);
moveMap.bind(keyboard, "numpad5", selectFavorite5);
moveMap.bind(keyboard, "numpad6", selectFavorite6);
moveMap.bind(keyboard, "p", programObject);
moveMap.bind(keyboard, "i", insObject);
moveMap.bind(keyboard, "f2", showPowerGrid);
moveMap.bind(keyboard, "f1", showSpawner);
moveMap.bind(keyboard, "f5", showShipData);
moveMap.bind(mouse0, "xaxis", yaw);
moveMap.bind(mouse0, "yaxis", pitch);
moveMap.bind(mouse0, "zaxis", zoomWheel);