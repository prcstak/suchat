import React from "react";

const Message = ({isMine, message, timestamp}) => {
    return (
        isMine? <div style={{ backgroundColor: "lightblue", borderRadius: 15, paddingLeft: 10, marginLeft:"40%", margin: 10}}>
            <div className="timestamp">
                {timestamp}
            </div>
            <div className="bubble-container">
                <div className="bubble" title={timestamp}>
                    {message}
                </div>
            </div>
        </div> :
            <div style={{backgroundColor: "wheat", borderRadius: 15, paddingLeft: 10, marginRight:"40%", margin: 10}}>
                <div className="timestamp">
                    {timestamp}
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