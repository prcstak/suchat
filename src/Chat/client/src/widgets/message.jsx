import React from "react";

const Message = ({user, isMine, message, timestamp}) => {
    return (
        isMine? <div style={{ backgroundColor: "lightblue", borderRadius: 15, paddingLeft: 10, margin: "2% 2% 5% 40%"}}>
            <div className="timestamp">
                {user}--{timestamp}
            </div>
            <div className="bubble-container">
                <div className="bubble" title={timestamp}>
                    {message}
                </div>
            </div>
        </div> :
            <div style={{backgroundColor: "wheat", borderRadius: 15, paddingLeft: 10,  margin: "2% 40% 2% 2%"}}>
                <div className="timestamp">
                    {user}--{timestamp}
                </div>
                <div className="bubble-container">
                    <div className="bubble" title={timestamp}>
                        {message}
                    </div>
                </div>
            </div>
    );
};

export default Message;