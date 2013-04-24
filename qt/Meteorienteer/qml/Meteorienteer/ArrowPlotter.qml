// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1

Rectangle {
    id: arrowPlotter
    anchors.fill: parent;
    signal directionSelected(int direction);
    property int direction: 0;
    property alias imageUrl: capturedImage.source;
    Image {
        id: arrowOk
        x:0; y:0;width:500;height:100;
        source: "qrc:/gfx/buttonNormal.png"
        Text {
            anchors.left: parent.left
            anchors.top: parent.top
            id: arrowText
            text: qsTr("Draw flight direction and press ok")
        }
         z:10;
        MouseArea {
            anchors.fill: parent;
            onPressed: {
                console.log("send arrow rotation" + arrow.rotation+90);
                directionSelected(arrow.rotation+90);
            }
        }
    }
    Image {
        id: arrow
        x:100; y:100;width:50;height:10;
        fillMode: Image.Stretch;
        transformOrigin: "Left"; smooth: false;
        source: "qrc:/gfx/arrow2.gif"
        z:5;
        visible:true;
        opacity: 50
    }
    Image {
        id: capturedImage;
        MouseArea {
            id: mouse_area1
            anchors.fill: parent
            property int mxi;
            property int myi;
            property int trigdelta: 20;

            onPressed: {
                myi=mouseY;
                mxi=mouseX;
                console.debug( "mouse("+mxi+","+myi+") pressed");
            }
            onReleased: {
                console.debug( "mouse("+mouseX+","+mouseY+") released");
                console.debug( "mouse move delta:" + movedelta(mxi, myi, mouseX, mouseY) );
                arrow.visible=true;
            }
            onMousePositionChanged: {
                if(movedelta(mxi,myi, mouseX, mouseY)>trigdelta && pressed) {
                    arrow.x=mxi; arrow.y=myi;
                    arrow.visible=true;
                    arrow.x = mxi; arrow.y = myi;
                    arrow.transformOrigin = "left";
                    arrow.rotation = Math.atan2(mouseY-myi,mouseX-mxi)*180/(Math.PI);
                    arrow.width = movedelta(mxi,myi,mouseX,mouseY);
                } else arrow.visible=false;
            }
        }
        anchors.fill: parent
        source: "qrc:/gfx/arrow2.gif"
    }
    function movedelta( mxi, myi, mxj, myj) {
        return Math.sqrt( Math.pow(mxj-mxi,2) + Math.pow(myj-myi,2) );
    }
}
