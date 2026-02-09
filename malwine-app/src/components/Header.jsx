import React from 'react';
import { Link } from 'react-router-dom'; 
import logo from '../assets/img/logo.png';

function Header() {
  return (
    <header>
      <Link to="./" className="logo"> 
        <img src={logo} alt="Logo" width="58" height="58" />
        <h1>Malwine ఌ Movie</h1>
      </Link>
    </header>
  );
}

export default Header;
