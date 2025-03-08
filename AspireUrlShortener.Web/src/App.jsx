import { useState } from 'react'

import './App.css'
import axios from '../node_modules/axios/index';

function App() {
    const [shortCode, setShortCode] = useState("")
    const [urlToShorten, setUrlToShorten] = useState("")
    const [newShortUrl, setNewShortUrl] = useState("")
    const [copyMessage, setCopyMessage] = useState('')

    const goToUrl = (shortCode) => {
        window.location.href = `/r/${shortCode}`;
    }

    const shortenUrl = (url) => {
        axios
            .post("/api/shorten", { url })
            .then((resp) => {
                setNewShortUrl(window.location.href + `r/${resp.data.shortCode}`);
            })
            .catch((reason) => console.log('Failed to Shorten Url', reason))
    }

    const Result = () => {
        const iconUrl = 'https://cdn-icons-png.flaticon.com/16/126/126498.png';
        const copyUrl = () => {
            navigator.clipboard.writeText(newShortUrl);
            setCopyMessage('Copied!');
        }

        if (newShortUrl.length === 0)
            return '';

        return (
            <div>
                <p>Your Url was shortened to:</p>
                {newShortUrl}
                <img src={iconUrl} alt="copy" onClick={copyUrl} />
                <p>{copyMessage}</p>
            </div>
        )
    }

    return (
        <>
            <h1>Url Shortener</h1>
            <br />
            <div>
                <h2>Follow Url</h2>
                <input value={shortCode} onChange={(event) => setShortCode(event.target.value)} />
                <br />
                <button onClick={() => goToUrl(shortCode)}> Go To Url </button>
            </div>
            <br />
            <div>
                <h2>Shorten New Url</h2>
                <input value={urlToShorten} onChange={(event) => setUrlToShorten(event.target.value)} />
                <br />
                <button onClick={() => shortenUrl(urlToShorten)}> Shorten </button>
                <Result />
            </div>
        </>
    )
}

export default App
