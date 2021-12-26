// SPDX-License-Identifier: MIT
pragma solidity >=0.4.22 <0.9.0;

import "@openzeppelin/contracts/utils/Strings.sol";

contract Vote {
    uint256 public candidate1;
    uint256 public candidate2;
    mapping(address => bool) public voted;

    function castVote(uint256 candidate) public {
        require(!voted[msg.sender] && (candidate == 1 || candidate == 2));
        if (candidate == 1) {
            candidate1++;
        } else {
            candidate2++;
        }
        voted[msg.sender] = true;
    }
}
